namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class MedicalCardRepository : IMedicalCardRepository
    {
        public MedicalCardRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<MedicalCard> GetMedicalCardPagesAsync(string login, int pageIndex, int pageSize = 20)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT [Id] FROM [User] WHERE [Login] = @login) 
		                    BEGIN
			                    DECLARE @userId AS INT
								SELECT TOP(1) @userId = [Id] FROM [User] WHERE [Login] = @login

								IF NOT EXISTS (SELECT [Id] FROM [MedicalCard] WHERE [UserId] = @userId) 
								BEGIN
									INSERT INTO [MedicalCard]([UserId]) VALUES (@userId)
								END

								DECLARE @cardId AS INT
								SELECT TOP(1) @cardId = [Id] FROM [MedicalCard] WHERE [UserId] = @userId

								DECLARE @count AS INT
								SELECT @count = COUNT(*) FROM [MedicalCardRecord] WHERE [MedicalCardId] = @cardId

				                SELECT 
								MC.[Id], 
								[StartedAtUtc], 
								@count AS TotalRecords,
								U.[Id], U.[Login], NULL AS [PasswordHash], 
								MCR.[Id], 
								MCR.[AddedAtUtc], MCR.[ModifiedAtUtc], MCR.[Content],
								MCR.[AssociatedRecordId], 
								UD.[Id], UD.[Login], NULL AS PasswordHash, D.[Info], D.[CabinetNumber], HI.[Id], HI.[Name], MS.[Id], MS.[Name], MS.[Description] 
								
								FROM [MedicalCard] MC 
								LEFT JOIN [User] U 
								ON 
								MC.[UserId] = U.[Id] 
								LEFT JOIN [MedicalCardRecord] MCR 
								ON 
								MCR.[MedicalCardId] = MC.[Id] 
								LEFT JOIN [Doctor] D 
								ON 
								D.[UserId] = MCR.[DoctorId] 
								LEFT JOIN 
								[User] UD 
								ON 
								UD.[Id] = D.[UserId]
								LEFT JOIN [HealthcareInstitution] HI
								ON D.[HealthcareInstitutionId] = HI.[Id]
								LEFT JOIN [MedicalSpecialization] MS 
								ON D.[MedicalSpecializationId] = MS.[Id]
								WHERE U.[Id] = @userId
								
                                ORDER BY MCR.[AddedAtUtc] ASC
								OFFSET @offset ROWS
								FETCH NEXT @pageSize ROWS ONLY
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('There is no such user', 16, 1)
		                    END
                    COMMIT TRAN";

                    IEnumerable<MedicalCard> cards =
                        await
                        connection.QueryAsync<MedicalCard, User, MedicalCardRecord, Doctor, HealthcareInstitution, MedicalSpecialization, MedicalCard>(
                            command,
                            (card, user, record, doctor, institution, specialization) =>
                            {
                                if (card.Records == null)
                                {
                                    card.Records = new List<MedicalCardRecord>();
                                }

                                if (doctor != null)
                                {
                                    doctor.Institution = institution;
                                    doctor.Specialization = specialization;

                                    if (record != null)
                                    {
                                        record.Author = doctor;
                                        card.Records.Add(record);
                                    }
                                }

                                card.User = user;

                                return card;
                            }, 
                            new { login, offset = pageSize * pageIndex, pageSize });

                    var medicalCard = cards.GroupBy(o => o.Id).Select(
                        group =>
                        {
                            var cardTemplate = group.First();
                            cardTemplate.Records =
                                new List<MedicalCardRecord>(
                                    group.Select(gr => gr.Records.FirstOrDefault()).Where(record => record != null));

                            return cardTemplate;
                        }).FirstOrDefault();

                    return medicalCard;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
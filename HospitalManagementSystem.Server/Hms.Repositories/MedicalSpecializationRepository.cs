namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class MedicalSpecializationRepository : IMedicalSpecializationRepository
    {
        public MedicalSpecializationRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<MedicalSpecialization> GetMedicalSpecializationAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    [Id],
	                [Name],
                    [Description]
                    FROM [MedicalSpecialization]
                    WHERE [Id] = @id";

                    var specialization = await connection.QueryFirstOrDefaultAsync<MedicalSpecialization>(command, new { id });
                    
                    return specialization;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateMedicalSpecializationAsync(MedicalSpecialization specialization)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [MedicalSpecialization] WHERE [Id] = @Id) 
		                    BEGIN
                                UPDATE [MedicalSpecialization] WITH (SERIALIZABLE) 
						            SET 
                                        [Name] = @Name,
                                        [Description] = @Description
	                                WHERE [Id] = @Id
                                SELECT @Id
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [MedicalSpecialization] ([Name], [Description]) OUTPUT INSERTED.ID VALUES 
                                                        (@Name, @Description)
		                    END
                    COMMIT TRAN";

                    return await connection.ExecuteAsync(command, specialization);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<IEnumerable<MedicalSpecialization>> GetSpecializationsAsync(int institutionId, int pageIndex, int pageSize, string filter)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT MS.[Id], MS.[Description], MS.[Name]
                    FROM [Polyclinic] P 
                    LEFT JOIN [Doctor] D 
                    ON 
                    P.[Id] = D.[HealthcareInstitutionId]
                    LEFT JOIN [MedicalSpecialization] MS
                    ON
                    D.[MedicalSpecializationId] = MS.[Id]
                    WHERE P.[HeathcareInstitutionId] = 1 AND MS.[Name] LIKE '%' + @filter + '%'

                    ORDER BY MS.[Id]
                    OFFSET @offset ROWS
                    FETCH NEXT @pageSize ROWS ONLY";

                    return await connection.QueryAsync<MedicalSpecialization>(command, new { institutionId, offset = pageSize * pageIndex, pageSize, filter });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> GetSpecializationsCountAsync(int institutionId, string filter)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT COUNT(*)
                    FROM [Polyclinic] P 
                    LEFT JOIN [Doctor] D 
                    ON 
                    P.[Id] = D.[HealthcareInstitutionId]
                    LEFT JOIN [MedicalSpecialization] MS
                    ON
                    D.[MedicalSpecializationId] = MS.[Id]
                    WHERE P.[HeathcareInstitutionId] = @institutionId AND MS.[Name] LIKE '%' + @filter + '%'";

                    return await connection.QueryFirstAsync<int>(command, new { institutionId, filter });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

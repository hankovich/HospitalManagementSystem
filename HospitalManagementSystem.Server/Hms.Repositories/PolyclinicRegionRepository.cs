namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class PolyclinicRegionRepository : IPolyclinicRegionRepository
    {
        public PolyclinicRegionRepository(string connectionString, IDoctorRepository doctorRepository)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.DoctorRepository = doctorRepository;
        }

        public string ConnectionString { get; set; }

        public IDoctorRepository DoctorRepository { get; }

        public async Task<PolyclinicRegion> GetRegionAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"

                    IF NOT EXISTS (SELECT [Id] FROM [PolyclinicRegion] WHERE [Id] = @Id) 
					    BEGIN
						    RAISERROR ('There is no such region', 16, 1)
                        END

                    SELECT 
                    PR.[Id],
	                [PolyclinicId],
	                [RegionNumber],
					UD.[Id], UD.[Login], NULL AS PasswordHash, D.[Info], D.[CabinetNumber], HI.[Id], HI.[Name], MS.[Id], MS.[Name], MS.[Description]
	                FROM [PolyclinicRegion] PR
                    LEFT JOIN [Doctor] D 
                    ON
                    PR.[RegionHeadId] = D.[UserId]
					LEFT JOIN 
					[User] UD 
					ON 
					UD.[Id] = D.[UserId]
					LEFT JOIN [HealthcareInstitution] HI
					ON D.[HealthcareInstitutionId] = HI.[Id]
					LEFT JOIN [MedicalSpecialization] MS 
					ON D.[MedicalSpecializationId] = MS.[Id]
					WHERE PR.[Id] = 1";

                    var polyclinicRegions =
                        await connection
                            .QueryAsync<PolyclinicRegion, Doctor, HealthcareInstitution, MedicalSpecialization, PolyclinicRegion>(
                                command,
                                (region, doctor, institution, specialization) =>
                                {
                                    doctor.Institution = institution;
                                    doctor.Specialization = specialization;

                                    region.RegionHead = doctor;

                                    return region;
                                },
                                new { id });

                    return polyclinicRegions.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<PolyclinicRegion> GetRegionAsync(int polyclinicId, int regionNumber)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    [Id],
	                WHERE [PolyclinicId] = @polyclinicId AND [RegionNumber] = @regionNumber";

                    int polyclinicRegionId = await connection.QueryFirstOrDefaultAsync<int>(command, new { polyclinicId, regionNumber });

                    if (polyclinicRegionId == default(int))
                    {
                        throw new ArgumentException("There are no such region");
                    }

                    return await this.GetRegionAsync(polyclinicRegionId);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateRegionAsync(PolyclinicRegion polyclinicRegion)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    int doctorId = await this.DoctorRepository.InsertOrUpdateDoctorAsync(polyclinicRegion.RegionHead);

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [PolyclinicRegion] WHERE [Id] = @Id) 
		                    BEGIN
                                UPDATE [PolyclinicRegion] WITH (SERIALIZABLE) 
						            SET 
                                        [PolyclinicId] = @PolyclinicId,
	                                    [RegionNumber] = @RegionNumber,
                                        [RegionHeadId] = @doctorId
						                WHERE [Id] = @Id
                                        SELECT @Id
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [Profile] ([PolyclinicId], [RegionNumber], [RegionHeadId]) OUTPUT INSERTED.ID VALUES 
                                                        (@PolyclinicId, @RegionNumber, @doctorId)
		                    END
                    COMMIT TRAN";

                    return await connection.ExecuteAsync(command, new { polyclinicRegion, doctorId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

namespace Hms.Repositories
{
    using System;
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
			                    INSERT INTO [HealthcareInstitution] ([Name], [Description]) OUTPUT INSERTED.ID VALUES 
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
    }
}

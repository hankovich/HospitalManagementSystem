namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class HealthcareInstitutionRepository : IHealthcareInstitutionRepository
    {
        public HealthcareInstitutionRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<HealthcareInstitution> GetHealthcareInstitutionAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    [Id],
	                [Name]
                    FROM [HealthcareInstitution]
                    WHERE [Id] = @id";

                    var institution = await connection.QueryFirstOrDefaultAsync<HealthcareInstitution>(command, new { id });

                    return institution;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateHealthcareInstitutionAsync(HealthcareInstitution institution)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [HealthcareInstitution] WHERE [Id] = @Id) 
		                    BEGIN
                                UPDATE [HealthcareInstitution] WITH (SERIALIZABLE) 
						            SET 
                                        [Name] = @Name,
	                                WHERE [Id] = @Id
                                SELECT @Id
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [HealthcareInstitution] ([Name]) OUTPUT INSERTED.ID VALUES 
                                                        (@Name)
		                    END
                    COMMIT TRAN";

                    return await connection.ExecuteAsync(command, institution);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

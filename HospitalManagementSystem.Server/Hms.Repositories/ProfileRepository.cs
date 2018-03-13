namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class ProfileRepository : IProfileRepository
    {
        public ProfileRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<Profile> GetProfileAsync(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    [Id],
	                [UserId],
	                [FirstName],
	                [MiddleName],
	                [LastName],
	                [DateOfBirth],
	                [Phone],
	                [BuildingId],
	                [Entrance],
	                [Floor],
	                [Flat],
	                [Photo]
                    FROM [Profile]
                    WHERE [UserId] = @userId";

                    var profile = await connection.QueryFirstOrDefaultAsync<Profile>(command, new { userId });

                    return profile;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateProfileAsync(Profile profile)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [Profile] WHERE [UserId] = @UserId) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [Profile] WHERE [UserId] = @UserId AND [Id] = @Id) 
			                        BEGIN
                                        UPDATE [Profile] WITH (SERIALIZABLE) 
						                           SET 
                                                   [FirstName] = @FirstName,
	                                               [MiddleName] = @MiddleName,
	                                               [LastName] = @LastName,
	                                               [DateOfBirth] = @DateOfBirth,
	                                               [Phone] = @Phone,
	                                               [BuildingId] = @BuildingId,
	                                               [Entrance] = @Entrance,
	                                               [Floor] = @Floor,
	                                               [Flat] = @Flat,
	                                               [Photo] = @Photo
						                           WHERE [UserId] = @UserId AND [Id] = @Id
                                        SELECT @Id
			                        END
			                    ELSE
			                        BEGIN
				                        RAISERROR ('There is another profile for this user', 16, 1);  
			                        END
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [Profile] ([UserId], [FirstName], [MiddleName], [LastName], [DateOfBirth], [Phone], [BuildingId], [Entrance], [Floor], [Flat], [Photo]) OUTPUT INSERTED.ID VALUES 
                                                        (@UserId, @FirstName, @MiddleName, @LastName, @DateOfBirth, @Phone, @BuildingId, @Entrance, @Floor, @Flat, @Photo)
		                    END
                    COMMIT TRAN";

                    return await connection.ExecuteAsync(command, profile);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

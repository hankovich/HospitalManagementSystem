namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Repositories.Interface;

    public class UserSessionRepository : IUserSessionRepository
    {
        public UserSessionRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task AddEntryAsync(string login, string modelIndentifier)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    DECLARE @userId AS INT
	                    SELECT TOP(1) @userId = [Id] FROM [User] WHERE [Login] = @login
	
	                    DECLARE @gadgetId AS INT
	                    SELECT TOP(1) @gadgetId = [Id] FROM [Gadget] WHERE [Identifier] = @modelIndentifier

	                    IF EXISTS (SELECT [Id] FROM [UserGadgetSession] WHERE [UserId] != @userId AND [GadgetId] = @gadgetId)
		                    BEGIN
			                    DELETE FROM [UserGadgetSession] WHERE [UserId] != @userId AND [GadgetId] = @gadgetId
		                    END

	                    IF EXISTS (SELECT [Id] FROM [UserGadgetSession] WHERE [UserId] = @userId AND [GadgetId] = @gadgetId)
		                    BEGIN
			                    UPDATE [UserGadgetSession] SET [LastRequestDateUtc] = GETUTCDATE() WHERE [UserId] = @userId AND [GadgetId] = @gadgetId
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [UserGadgetSession]([UserId], [GadgetId], [StartDateUtc], [LastRequestDateUtc]) VALUES (@userId, @gadgetId, GETUTCDATE(), GETUTCDATE())
		                    END
                    COMMIT TRAN";

                    await connection.ExecuteAsync(command, new { login, modelIndentifier });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task AddEntryAsync(int userId, int gadgetId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT [Id] FROM [UserGadgetSession] WHERE [UserId] != @userId AND [GadgetId] = @gadgetId)
		                    BEGIN
			                    DELETE FROM [UserGadgetSession] WHERE [UserId] != @userId AND [GadgetId] = @gadgetId
		                    END

	                    IF EXISTS (SELECT [Id] FROM [UserGadgetSession] WHERE [UserId] = @userId AND [GadgetId] = @gadgetId)
		                    BEGIN
			                    UPDATE [UserGadgetSession] SET [LastRequestDateUtc] = GETUTCDATE() WHERE [UserId] = @userId AND [GadgetId] = @gadgetId
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [UserGadgetSession]([UserId], [GadgetId], [StartDateUtc], [LastRequestDateUtc]) VALUES (@userId, @gadgetId, GETUTCDATE(), GETUTCDATE())
		                    END
                    COMMIT TRAN";

                    await connection.ExecuteAsync(command, new { userId, gadgetId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<IEnumerable<string>> GetEntriesAsync(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT [GadgetId] FROM [UserGadgetSession] WHERE [UserId] = @userId";

                    return await connection.QueryAsync<string>(command, new { userId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

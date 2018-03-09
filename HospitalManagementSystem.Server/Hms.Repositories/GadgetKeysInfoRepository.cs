namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Models;
    using Hms.Repositories.Interface;

    public class GadgetKeysInfoRepository : IGadgetKeysInfoRepository
    {
        public GadgetKeysInfoRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<byte[]> GetGadgetRoundKeyAsync(string gadgetIdentifier)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    DECLARE @gadgetIdentifierId AS INT
									SELECT TOP(1) @gadgetIdentifierId = [Id] FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier

				                    SELECT [RoundKey] FROM [GadgetRoundKey] WHERE [GadgetIdentifierId] = @gadgetIdentifierId
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1)
		                    END
                    COMMIT TRAN";
                    return await connection.QuerySingleAsync<byte[]>(command, new { gadgetIdentifier });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<byte[]> GetGadgetPublicKeyAsync(string gadgetIdentifier, string clientSecret)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret) 
			                    BEGIN
                                    SELECT [PublicKey] FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret
			                    END
			                    ELSE
			                    BEGIN
				                    RAISERROR ('Invalid client secret', 16, 1);  
			                    END
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1); 
		                    END
                    COMMIT TRAN";

                    return await connection.QuerySingleAsync<byte[]>(command, new { gadgetIdentifier, clientSecret });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<string> GetGadgetClientSecretAsync(string gadgetIdentifier)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    SELECT TOP(1) [ClientSecret] FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1)
		                    END
                    COMMIT TRAN";

                    return await connection.QuerySingleAsync<string>(command, new { gadgetIdentifier });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<KeysInfoModel> GetGadgetKeysInfoAsync(string gadgetIdentifier, string clientSecret)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret) 
			                    BEGIN
                                    SELECT TOP(1) [GadgetIdentifier].[PublicKey], [GadgetRoundKey].[RoundKey], [GadgetRoundKey].[SentTimes] AS RoundKeySentTimes, [GadgetRoundKey].[GeneratedAtUTC] AS GeneratedTimeUtc FROM [GadgetIdentifier] JOIN [GadgetRoundKey] ON [GadgetRoundKey].[GadgetIdentifierId] = [GadgetIdentifier].[Id]
                                    WHERE [GadgetIdentifier].[Identifier] = @gadgetIdentifier;
			                    END
			                    ELSE
			                    BEGIN
				                    RAISERROR ('Invalid client secret', 16, 1);  
			                    END
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1); 
		                    END
                    COMMIT TRAN";

                    return await connection.QuerySingleAsync<KeysInfoModel>(command, new { gadgetIdentifier, clientSecret });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task IncrementGadgetRoundKeySentTimesAsync(string gadgetIdentifier, string clientSecret)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret) 
			                    BEGIN
									DECLARE @gadgetIdentifierId AS INT
									DECLARE @sentTimes AS INT
									SELECT TOP(1) @gadgetIdentifierId = [Id] FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret
                                    SELECT TOP(1) @sentTimes = [SentTimes] FROM [GadgetRoundKey] WHERE [GadgetIdentifierId] = @gadgetIdentifierId
				                    
										UPDATE [GadgetRoundKey] WITH (SERIALIZABLE) SET 
											   [SentTimes] = @sentTimes + 1
						                       WHERE [GadgetIdentifierId] = @gadgetIdentifierId
			                    END
			                    ELSE
			                    BEGIN
				                    RAISERROR ('Invalid client secret', 16, 1);  
			                    END
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1)
		                    END
                    COMMIT TRAN";

                    await connection.ExecuteAsync(command, new { gadgetIdentifier, clientSecret });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task SetGadgetPublicKeyAsync(string gadgetIdentifier, string clientSecret, byte[] publicKey)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret) 
			                        BEGIN
				                        UPDATE [GadgetIdentifier] WITH (SERIALIZABLE) SET 
						                           [PublicKey] = @publicKey
						                           WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret
			                        END
			                    ELSE
			                        BEGIN
				                        RAISERROR ('Invalid client secret', 16, 1);  
			                        END
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [GadgetIdentifier] ([Identifier], [ClientSecret], [PublicKey]) VALUES (@gadgetIdentifier, @clientSecret, @publicKey)
		                    END
                    COMMIT TRAN";

                    await connection.ExecuteAsync(command, new { gadgetIdentifier, clientSecret, publicKey });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task SetGadgetRoundKey(string gadgetIdentifier, string clientSecret, byte[] roundKey)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier) 
		                    BEGIN
			                    IF EXISTS (SELECT * FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret) 
			                    BEGIN
									DECLARE @gadgetIdentifierId AS INT
									SELECT TOP(1) @gadgetIdentifierId = [Id] FROM [GadgetIdentifier] WHERE [Identifier] = @gadgetIdentifier AND [ClientSecret] = @clientSecret

                                    IF NOT EXISTS (SELECT * FROM [GadgetRoundKey] WHERE [GadgetIdentifierId] = @gadgetIdentifierId)
                                        BEGIN
                                            INSERT INTO [GadgetRoundKey] ([GadgetIdentifierId], [RoundKey], [SentTimes]) VALUES (@gadgetIdentifierId, @roundKey, 0) 
                                        END
                                    ELSE
                                        BEGIN
				                            UPDATE [GadgetRoundKey] WITH (SERIALIZABLE) SET 
						                               [RoundKey] = @roundKey,
											           [SentTimes] = 0,
                                                       [GeneratedAtUTC] = GETUTCDATE()
						                               WHERE [GadgetIdentifierId] = @gadgetIdentifierId
                                        END
			                    END
			                    ELSE
			                    BEGIN
                                    RAISERROR ('Invalid client secret', 16, 1);
			                    END
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('Invalid identifier', 16, 1)
		                    END
                    COMMIT TRAN";

                    await connection.ExecuteAsync(command, new { gadgetIdentifier, clientSecret, roundKey });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

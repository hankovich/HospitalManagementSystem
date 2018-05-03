namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class AttachmentRepository : IAttachmentRepository
    {
        public AttachmentRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task<Attachment> GetAttachmentAsync(string login, int attachmentId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
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

								SELECT A.[Id], A.[Name], A.[CreatedBy] AS [CreatedById], A.[CreatedAtUtc], A.[Content]
                                FROM [Attachment] A 
                                LEFT JOIN [MedicalCardRecord] MCR 
                                ON 
                                A.[MedicalCardRecordId] = MCR.[Id] 
                                LEFT JOIN [MedicalCard] MC 
                                ON 
                                MCR.[MedicalCardId] = MC.[Id] 
                                WHERE MC.[Id] = @cardId 
                                AND A.[Id] = @attachmentId
							END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('There is no such user', 16, 1)
		                    END";

                    return await connection.QuerySingleAsync<Attachment>(command, new { login, attachmentId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<AttachmentInfo> GetAttachmentInfoAsync(string login, int attachmentId)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
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

								SELECT A.[Id], A.[Name], A.[CreatedBy] AS [CreatedById], A.[CreatedAtUtc]
                                FROM [Attachment] A 
                                LEFT JOIN [MedicalCardRecord] MCR 
                                ON 
                                A.[MedicalCardRecordId] = MCR.[Id] 
                                LEFT JOIN [MedicalCard] MC 
                                ON 
                                MCR.[MedicalCardId] = MC.[Id] 
                                WHERE MC.[Id] = @cardId 
                                AND A.[Id] = @attachmentId
							END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('There is no such user', 16, 1)
		                    END";

                    return await connection.QuerySingleAsync<AttachmentInfo>(command, new { login, attachmentId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

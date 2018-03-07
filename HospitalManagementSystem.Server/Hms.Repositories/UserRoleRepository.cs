namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Repositories.Interface;

    public class UserRoleRepository : IUserRoleRepository
    {
        public UserRoleRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string login)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    R.[Name] 
                    FROM [User] U JOIN [UserRole] UR ON U.Id = UR.UserId JOIN [Role] R ON UR.RoleId = R.Id 
                    WHERE
                    U.UserName = @login   
                    ";

                    return await connection.QueryAsync<string>(
                        command,
                        new { login });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task AddRoleToUserAsync(string login, string rolename)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    DECLARE @userId AS INT
                    SELECT TOP(1) @userId = [Id] FROM [User] WHERE [UserName] = @login

                    DECLARE @roleId AS INT
                    SELECT TOP(1) @roleId = [Id] FROM [Role] WHERE [Name] = @rolename

                    IF @userId IS NULL
                    BEGIN
	                    RAISERROR ('There is no such user', 16, 1);
                    END

                    IF @roleId IS NULL
                    BEGIN
	                    RAISERROR ('There is no such role', 16, 1);
                    END

                    INSERT INTO [UserRole] ([UserId], [RoleId]) VALUES(@userId, @roleId)";

                    await connection.ExecuteAsync(
                        command,
                        new { login, rolename });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

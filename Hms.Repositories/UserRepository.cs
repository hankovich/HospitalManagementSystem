namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Repositories.Interface;

    using Microsoft.AspNet.Identity;

    public class UserRepository : IUserRepository
    {
        public UserRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public async Task AddUserAsync(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    INSERT INTO [User] ([UserName], [PasswordHash]) VALUES (@username, @passwordHash)";

                    await connection.ExecuteAsync(
                        command,
                        new { username, passwordHash = new PasswordHasher().HashPassword(password) });
                }
            }
            catch (SqlException exception)
            {
                bool uniqueTrouble = exception.Number == 2627;
                bool dataIsTooLong = exception.Number == 8152;

                string message;

                if (uniqueTrouble)
                {
                    message = "This username is already taken";
                }
                else if (dataIsTooLong)
                {
                    message = "Username is too long";
                }
                else
                {
                    message = "Something went wrong";
                }

                throw new ArgumentException(message);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT 
                    [User].[UserName], [User].[PasswordHash]
                    FROM [User]
                    WHERE [User].[UserName] = @username";

                    var usersInfo = (await connection.QueryAsync<User>(command, new { username })).ToList();

                    if (!usersInfo.Any())
                    {
                        throw new ArgumentException("There is no user with such username");
                    }

                    var hasher = new PasswordHasher();
                    try
                    {
                        var user = usersInfo.Single(
                            u => hasher.VerifyHashedPassword(u.PasswordHash, password)
                                 != PasswordVerificationResult.Failed);

                        return user;
                    }
                    catch
                    {
                        throw new Exception("Invalid password");
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
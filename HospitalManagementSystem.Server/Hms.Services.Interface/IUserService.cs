namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IUserService
    {
        Task AddUserAsync(string username, string password);

        Task<User> GetUserAsync(string username, string password);

        Task<bool> CheckCredentials(string username, string password);
    }
}

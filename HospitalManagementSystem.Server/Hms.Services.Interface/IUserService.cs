namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task AddUserAsync(string username, string password);

        Task<User> GetUserAsync(string username, string password);

        Task<bool> CheckCredentials(string username, string password);
    }
}

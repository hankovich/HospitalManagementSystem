namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IUserRepository
    {
        Task AddUserAsync(string username, string password);

        Task<User> GetUserAsync(string username, string password);

        Task<int> GetUserIdByLoginAsync(string username);
    }
}

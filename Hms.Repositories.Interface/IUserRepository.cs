namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task AddUserAsync(string username, string password);

        Task<User> GetUserAsync(string username, string password);
    }
}

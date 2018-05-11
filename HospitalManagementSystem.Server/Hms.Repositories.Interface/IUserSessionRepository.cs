namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    public interface IUserSessionRepository
    {
        Task AddEntryAsync(string login, string modelIndentifier);

        Task AddEntryAsync(int userId, int gadgetId);
    }
}

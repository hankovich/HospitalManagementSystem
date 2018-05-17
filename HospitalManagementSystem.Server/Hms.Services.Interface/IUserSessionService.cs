namespace Hms.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserSessionService
    {
        Task AddEntryAsync(string login, string modelIndentifier);

        Task AddEntryAsync(int userId, int gadgetId);

        Task<IEnumerable<string>> GetEntriesAsync(int userId);
    }
}

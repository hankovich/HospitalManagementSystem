namespace Hms.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class UserSessionService : IUserSessionService
    {
        public UserSessionService(IUserSessionRepository userSessionRepository)
        {
            this.UserSessionRepository = userSessionRepository;
        }

        public IUserSessionRepository UserSessionRepository { get; }

        public Task AddEntryAsync(string login, string modelIndentifier)
        {
            return this.UserSessionRepository.AddEntryAsync(login, modelIndentifier);
        }

        public Task AddEntryAsync(int userId, int gadgetId)
        {
            return this.UserSessionRepository.AddEntryAsync(userId, gadgetId);
        }

        public Task<IEnumerable<string>> GetEntriesAsync(int userId)
        {
            return this.UserSessionRepository.GetEntriesAsync(userId);
        }
    }
}

namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface.Infrastructure;

    public interface IProfileDataService
    {
        IRequestCoordinator Client { get; }

        Task<Profile> GetProfileAsync(int userId);

        Task<Profile> GetCurrentProfileAsync();

        Task<int> InsertOrUpdateProfileAsync(Profile profile);
    }
}

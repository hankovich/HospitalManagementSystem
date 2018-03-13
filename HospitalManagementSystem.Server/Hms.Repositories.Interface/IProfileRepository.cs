namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IProfileRepository
    {
        Task<Profile> GetProfileAsync(int userId);

        Task<int> InsertOrUpdateProfileAsync(Profile profile);
    }
}

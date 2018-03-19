namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IBuildingRepository
    {
        Task<BuildingAddress> GetBuildingAsync(int id);

        Task<int> GetBuildingIdOrDefaultAsync(double latitude, double longitude);

        Task<int> InsertOrUpdateBuildingAsync(BuildingAddress address);
    }
}

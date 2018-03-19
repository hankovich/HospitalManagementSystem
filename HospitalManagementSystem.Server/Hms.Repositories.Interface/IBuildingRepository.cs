namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IBuildingRepository
    {
        Task<BuildingAddress> GetBuildingAsync(int id);

        Task<int> InsertOrUpdateBuildingAsync(BuildingAddress address);
    }
}

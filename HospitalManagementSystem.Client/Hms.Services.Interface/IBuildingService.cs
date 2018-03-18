namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IBuildingService
    {
        Task<Profile> GetBuildingAsync(int buildingId);

        Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress);
    }
}

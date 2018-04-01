namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;

    public interface IBuildingDataService
    {
        Task<BuildingAddress> GetBuildingAsync(int buildingId);

        Task<BuildingAddress> GetBuildingAsync(GeoPoint geoPoint);

        Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress);
    }
}

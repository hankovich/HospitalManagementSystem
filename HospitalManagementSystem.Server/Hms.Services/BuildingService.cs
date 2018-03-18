namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;

    public class BuildingService : IBuildingService
    {
        public BuildingService(IPolyclinicRegionService polyclinicRegionService)
        {
            this.PolyclinicRegionService = polyclinicRegionService;
        }

        public IPolyclinicRegionService PolyclinicRegionService { get; set; }

        public Task<BuildingAddress> GetBuildingAsync(int buildingId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BuildingAddress> GetBuildingAsync(GeoPoint geoPoint)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress)
        {
            throw new System.NotImplementedException();
        }
    }
}

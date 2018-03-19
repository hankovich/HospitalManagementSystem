namespace Hms.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class BuildingService : IBuildingService
    {
        public BuildingService(IPolyclinicRegionService polyclinicRegionService, IBuildingRepository buildingRepository, IGeocoder geocoder, IPolyclinicRegionProvider polyclinicRegionProvider)
        {
            this.PolyclinicRegionService = polyclinicRegionService;
            this.BuildingRepository = buildingRepository;
            this.Geocoder = geocoder;
            this.PolyclinicRegionProvider = polyclinicRegionProvider;
        }

        public IPolyclinicRegionService PolyclinicRegionService { get; set; }

        public IBuildingRepository BuildingRepository { get; }

        public IGeocoder Geocoder { get; }

        public IPolyclinicRegionProvider PolyclinicRegionProvider { get; }

        public async Task<BuildingAddress> GetBuildingAsync(int buildingId)
        {
            return await this.BuildingRepository.GetBuildingAsync(buildingId);
        }

        public async Task<BuildingAddress> GetBuildingAsync(GeoPoint geoPoint)
        {
            GeoObjectCollection objectCollection =
                await this.Geocoder.ReverseGeocodeAsync(geoPoint, GeoObjectKind.House, 1, LangType.RU);

            GeoObject geoObject = objectCollection.First();
            Address address = geoObject.GeocoderMetaData.Address;

            int buildingId =
                await this.BuildingRepository.GetBuildingIdOrDefaultAsync(geoPoint.Latitude, geoPoint.Longitude);

            if (buildingId != default(int))
            {
                return await this.BuildingRepository.GetBuildingAsync(buildingId);
            }

            int id = await this.PolyclinicRegionProvider.GetPolyclinicRegionIdAsync(address);

            PolyclinicRegion region = await this.PolyclinicRegionService.GetRegionAsync(id);

            BuildingAddress building = new BuildingAddress
            {
                City = $"{address.Locality}, {address.Province}, {address.Country}",
                Street = address.Street,
                Building = address.House,
                Latitude = geoPoint.Latitude,
                Longitude = geoPoint.Longitude,
                PolyclinicRegion = region
            };

            int insertedBuildingId = await this.InsertOrUpdateBuildingAsync(building);
            building.Id = insertedBuildingId;

            return building;
        }

        public async Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress)
        {
            return await this.BuildingRepository.InsertOrUpdateBuildingAsync(buildingAddress);
        }
    }
}

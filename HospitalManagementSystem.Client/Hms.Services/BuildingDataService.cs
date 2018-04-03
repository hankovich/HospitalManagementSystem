namespace Hms.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;

    public class BuildingDataService : IBuildingDataService
    {
        public BuildingDataService(IRequestCoordinator client)
        {
            this.Client = client;
        }

        public IRequestCoordinator Client { get; set; }

        public async Task<BuildingAddress> GetBuildingAsync(int buildingId)
        {
            ServerResponse<BuildingAddress> response = await this.Client.SendAsync<BuildingAddress>(HttpMethod.Get, $"api/building/{buildingId}");

            if (response.Content == null)
            {
                throw new HmsException(response.ReasonPhrase);    
            }

            return response.Content;
        }

        public async Task<BuildingAddress> GetBuildingAsync(GeoPoint geoPoint)
        {
            ServerResponse<BuildingAddress> response = await this.Client.SendAsync<BuildingAddress>(HttpMethod.Get, $"api/building/{geoPoint.Latitude}/{geoPoint.Longitude}/");

            if (response.Content == null)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }

        public async Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress)
        {
            ServerResponse<int> response = await this.Client.SendAsync<int>(HttpMethod.Post, "api/building", buildingAddress);

            if (response.Content == default(int))
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }
    }
}

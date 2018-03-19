namespace Hms.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.Services.Interface;

    public class PolyclinicRegionService : IPolyclinicRegionService
    {
        public PolyclinicRegionService(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; set; }

        public async Task<PolyclinicRegion> GetPolyclinicRegionAsync(int polyclinicRegionId)
        {
            ServerResponse<PolyclinicRegion> response = await this.Client.SendAsync<PolyclinicRegion>(HttpMethod.Get, $"api/region/{polyclinicRegionId}");

            if (response.Content == null)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }

        public async Task<int> InsertOrUpdatePolyclinicRegionAsync(PolyclinicRegion polyclinicRegion)
        {
            ServerResponse<int> response = await this.Client.SendAsync<int>(HttpMethod.Post, "api/region", polyclinicRegion);

            if (response.Content == default(int))
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }
    }
}

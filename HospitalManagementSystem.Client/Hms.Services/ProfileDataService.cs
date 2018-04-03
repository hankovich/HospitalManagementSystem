namespace Hms.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class ProfileDataService : IProfileDataService
    {
        public ProfileDataService(IRequestCoordinator client)
        {
            this.Client = client;
        }

        public IRequestCoordinator Client { get; set; }

        public async Task<Profile> GetProfileAsync(int userId)
        {
            var response = await this.Client.SendAsync<Profile>(HttpMethod.Get, $"api/profile/{userId}");

            return response.Content;
        }

        public async Task<Profile> GetCurrentProfileAsync()
        {
            var response = await this.Client.SendAsync<Profile>(HttpMethod.Get, "api/profile");

            return response.Content;
        }

        public async Task<int> InsertOrUpdateProfileAsync(Profile profile)
        {
            var response = await this.Client.SendAsync<int>(HttpMethod.Put, "api/profile", profile);

            return response.Content;
        }
    }
}

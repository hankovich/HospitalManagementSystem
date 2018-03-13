namespace Hms.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class ProfileService : IProfileService
    {
        public ProfileService(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; set; }

        public async Task<Profile> GetProfileAsync(int userId)
        {
            var response = await this.Client.SendAsync<Profile>(HttpMethod.Get, $"api/profile/{userId}", null);

            return response.Content;
        }

        public async Task<Profile> GetCurrentProfileAsync()
        {
            var response = await this.Client.SendAsync<Profile>(HttpMethod.Get, "api/profile", null);

            return response.Content;
        }

        public async Task<int> InsertOrUpdateProfileAsync(Profile profile)
        {
            var response = await this.Client.SendAsync<int>(HttpMethod.Put, "api/profile", profile);

            return response.Content;
        }
    }
}

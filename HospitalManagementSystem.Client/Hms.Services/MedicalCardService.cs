namespace Hms.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class MedicalCardService : IMedicalCardService
    {
        public MedicalCardService(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; set; }

        public async Task<MedicalCard> GetMedicalCardAsync(int pageIndex, int pageSize = 20)
        {
            return (await this.Client.SendAsync<MedicalCard>(HttpMethod.Get, $"api/card/{pageIndex}/{pageSize}", null)).Content;
        }
    }
}

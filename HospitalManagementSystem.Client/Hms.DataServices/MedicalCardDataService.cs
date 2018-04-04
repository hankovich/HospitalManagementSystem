namespace Hms.DataServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;

    public class MedicalCardDataService : IMedicalCardDataService
    {
        public MedicalCardDataService(IRequestCoordinator client)
        {
            this.Client = client;
        }

        public IRequestCoordinator Client { get; set; }

        public async Task<MedicalCard> GetMedicalCardAsync(int pageIndex, int pageSize = 20)
        {
            var response = await this.Client.SendAsync<MedicalCard>(HttpMethod.Get, $"api/card/{pageIndex}/{pageSize}");

            if (response.Content == null)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }
    }
}

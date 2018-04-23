namespace Hms.DataServices
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class MedicalCardDataService : IMedicalCardDataService
    {
        public MedicalCardDataService(IRequestCoordinator client)
        {
            this.Client = client;
        }

        public IRequestCoordinator Client { get; }

        public async Task<MedicalCard> GetMedicalCardAsync(int pageIndex, int pageSize = 20, string filter = "")
        {
            var response = await this.Client.SendAsync<MedicalCard>(HttpMethod.Get, $"api/card/{pageIndex}/{pageSize}/{filter}");

            if (response.Content == null)
            {
                return new MedicalCard { TotalRecords = 0, Records = new List<MedicalCardRecord>() };
            }

            return response.Content;
        }
    }
}

﻿namespace Hms.DataServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class MedicalRecordDataService : IMedicalRecordDataService
    {
        public MedicalRecordDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<MedicalCardRecord> GetMedicalCardRecordAsync(int recordId)
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<MedicalCardRecord>(HttpMethod.Get, $"api/card/{recordId}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }
    }
}

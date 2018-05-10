namespace Hms.DataServices
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class MedicalSpecializationDataService : IMedicalSpecializationDataService
    {
        public MedicalSpecializationDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<MedicalSpecialization> GetMedicalSpecializationAsync(int specializationId)
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<MedicalSpecialization>(HttpMethod.Get, $"api/specialization/{specializationId}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }

        public async Task<IEnumerable<MedicalSpecialization>> GetMedicalSpecializationsAsync(int institutionId, int pageIndex, int pageSize = 20, string filter = "")
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<IEnumerable<MedicalSpecialization>>(HttpMethod.Get, $"api/specialization/{institutionId}/{pageIndex}/{pageSize}/{filter?.Trim()}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }

        public async Task<int> GetMedicalSpecializationCountAsync(int institutionId, string filter = "")
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<int>(HttpMethod.Get, $"api/specialization/count/{institutionId}/{filter?.Trim()}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }
    }
}

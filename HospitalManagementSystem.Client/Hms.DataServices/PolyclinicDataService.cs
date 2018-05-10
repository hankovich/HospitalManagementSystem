namespace Hms.DataServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class PolyclinicDataService : IPolyclinicDataService
    {
        public PolyclinicDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<Polyclinic> GetPolyclinicAsync(int polyclinicId)
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<Polyclinic>(HttpMethod.Get, $"api/polyclinic/{polyclinicId}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }
    }
}

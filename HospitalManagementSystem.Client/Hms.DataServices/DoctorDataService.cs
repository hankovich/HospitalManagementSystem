namespace Hms.DataServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class DoctorDataService : IDoctorDataService
    {
        public DoctorDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<Doctor> GetDoctorAsync(int doctorId)
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<Doctor>(HttpMethod.Get, $"api/doctor/{doctorId}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }
    }
}

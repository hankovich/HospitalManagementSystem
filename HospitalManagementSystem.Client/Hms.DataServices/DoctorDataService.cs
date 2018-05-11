namespace Hms.DataServices
{
    using System.Collections.Generic;
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

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync(
            int polyclinicId,
            int specializationId,
            int pageIndex,
            int pageSize = 20,
            string filter = "")
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<IEnumerable<Doctor>>(
                                     HttpMethod.Get,
                                     $"api/doctor/{polyclinicId}/{specializationId}/{pageIndex}/{pageSize}/{filter?.Trim()}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new HmsException(serverResponse.ReasonPhrase);
            }

            return serverResponse.Content;
        }

        public async Task<int> GetDoctorsCountAsync(int polyclinicId, int specializationId, string filter)
        {
            var serverResponse = await this.RequestCoordinator.SendAsync<int>(
                                     HttpMethod.Get,
                                     $"api/doctor/count/{polyclinicId}/{specializationId}/{filter?.Trim()}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                return 0;
            }

            return serverResponse.Content;
        }
    }
}

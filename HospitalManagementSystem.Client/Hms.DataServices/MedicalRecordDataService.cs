namespace Hms.DataServices
{
    using System;
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

            //return Task.FromResult(new MedicalCardRecord
            //{
            //    Content = "Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo \nHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nqwe",
            //    AddedAtUtc = DateTime.Now,
            //    ModifiedAtUtc = DateTime.Now,
            //    Id = recordId,
            //    Author = new Doctor
            //    {
            //        Login = "Doctor Proctor",
            //        Id = 19,
            //        CabinetNumber = 404,
            //        Info = "Information",
            //        Institution = new HealthcareInstitution
            //        {
            //            Id = recordId,
            //            Name = "NNNNNNNNNNNNName"
            //        },
            //        Specialization = new MedicalSpecialization
            //        {
            //            Id = recordId,
            //            Name = "NAAAAAAAAAAAAAAAAAAAAme",
            //            Description = "Long description"
            //        }
            //    }
            //});
        }
    }
}

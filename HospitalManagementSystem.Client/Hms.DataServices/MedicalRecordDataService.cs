namespace Hms.DataServices
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;

    public class MedicalRecordDataService : IMedicalRecordDataService
    {
        public Task<MedicalCardRecord> GetMedicalCardRecordAsync(int recordId)
        {
            return Task.FromResult(new MedicalCardRecord
            {
                Content = "Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo Helloo \nHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nHello\nqwe",
                AddedAtUtc = DateTime.Now,
                ModifiedAtUtc = DateTime.Now,
                Id = recordId,
                Author = new Doctor
                {
                    Login = "Doctor Proctor",
                    Id = 19,
                    CabinetNumber = 404,
                    Info = "Information",
                    Institution = new HealthcareInstitution
                    {
                        Id = recordId,
                        Name = "NNNNNNNNNNNNName"
                    },
                    Specialization = new MedicalSpecialization
                    {
                        Id = recordId,
                        Name = "NAAAAAAAAAAAAAAAAAAAAme",
                        Description = "Long description"
                    }
                }
            });
        }
    }
}

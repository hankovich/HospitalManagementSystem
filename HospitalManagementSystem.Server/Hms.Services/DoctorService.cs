namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class DoctorService : IDoctorService
    {
        public DoctorService(IDoctorRepository doctorRepository)
        {
            this.DoctorRepository = doctorRepository;
        }

        public IDoctorRepository DoctorRepository { get; }

        public Task<Doctor> GetDoctorAsync(int id)
        {
            return this.DoctorRepository.GetDoctorAsync(id);
        }

        public Task<int> InsertOrUpdateDoctorAsync(Doctor doctor)
        {
            return this.DoctorRepository.InsertOrUpdateDoctorAsync(doctor);
        }
    }
}

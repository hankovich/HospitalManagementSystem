namespace Hms.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorService
    {
        Task<Doctor> GetDoctorAsync(int id);

        Task<int> InsertOrUpdateDoctorAsync(Doctor doctor);

        Task<IEnumerable<Doctor>> GetDoctorsAsync(int polyclinicId, int specializationId, int pageIndex, int pageSize, string filter);

        Task<int> GetDoctorsCountAsync(int polyclinicId, int specializationId, string filter);
    }
}

namespace Hms.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorAsync(int id);

        Task<int> InsertOrUpdateDoctorAsync(Doctor doctor);

        Task<IEnumerable<Doctor>> GetDoctorsAsync(int polyclinicId, int specializationId, int pageIndex, int pageSize, string filter);

        Task<int> GetDoctorsCountAsync(int polyclinicId, int specializationId, string filter);

        Task<IEnumerable<int>> GetDoctorIdsAsync(IEnumerable<int> participants);
    }
}

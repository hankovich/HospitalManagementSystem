namespace Hms.DataServices.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorDataService
    {
        Task<Doctor> GetDoctorAsync(int doctorId);

        Task<IEnumerable<Doctor>> GetDoctorsAsync(int polyclinicId, int specializationId, int pageIndex, int pageSize = 20, string filter = "");

        Task<int> GetDoctorsCountAsync(int polyclinicId, int specializationId, string filter);
    }
}

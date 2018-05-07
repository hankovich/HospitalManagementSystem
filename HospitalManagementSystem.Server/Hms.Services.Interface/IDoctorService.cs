namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorService
    {
        Task<Doctor> GetDoctorAsync(int id);

        Task<int> InsertOrUpdateDoctorAsync(Doctor doctor);
    }
}

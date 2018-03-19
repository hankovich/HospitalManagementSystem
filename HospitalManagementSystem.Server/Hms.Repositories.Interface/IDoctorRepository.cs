namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorAsync(int id);

        Task<int> InsertOrUpdateDoctorAsync(Doctor doctor);
    }
}

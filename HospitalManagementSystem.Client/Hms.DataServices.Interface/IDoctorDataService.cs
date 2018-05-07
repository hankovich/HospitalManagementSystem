namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IDoctorDataService
    {
        Task<Doctor> GetDoctorAsync(int doctorId);
    }
}

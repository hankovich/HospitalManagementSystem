namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IHealthcareInstitutionRepository
    {
        Task<HealthcareInstitution> GetHealthcareInstitutionAsync(int id);

        Task<int> InsertOrUpdateHealthcareInstitutionAsync(HealthcareInstitution institution);
    }
}

namespace Hms.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalSpecializationService
    {
        Task<IEnumerable<MedicalSpecialization>> GetSpecializationsAsync(int institutionId, int pageIndex, int pageSize, string filter);

        Task<int> GetSpecializationsCountAsync(int institutionId, string filter);

        Task<MedicalSpecialization> GetSpecializationAsync(int specializationId);
    }
}

namespace Hms.DataServices.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalSpecializationDataService
    {
        Task<MedicalSpecialization> GetMedicalSpecializationAsync(int id);

        Task<IEnumerable<MedicalSpecialization>> GetMedicalSpecializationsAsync(int institurionId, int pageIndex, int pageSize = 20, string filter = "");

        Task<int> GetMedicalSpecializationCountAsync(int institurionId, string filter = "");
    }
}

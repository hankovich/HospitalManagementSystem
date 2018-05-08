namespace Hms.DataServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class MedicalSpecializationDataService : IMedicalSpecializationDataService
    {
        public MedicalSpecializationDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public Task<MedicalSpecialization> GetMedicalSpecializationAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MedicalSpecialization>> GetMedicalSpecializationsAsync(int institurionId, int pageIndex, int pageSize = 20, string filter = "")
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetMedicalSpecializationCountAsync(int institurionId, string filter = "")
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace Hms.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class MedicalSpecializationService : IMedicalSpecializationService
    {
        public MedicalSpecializationService(IMedicalSpecializationRepository medicalSpecializationRepository)
        {
            this.MedicalSpecializationRepository = medicalSpecializationRepository;
        }

        public IMedicalSpecializationRepository MedicalSpecializationRepository { get; }

        public async Task<IEnumerable<MedicalSpecialization>> GetSpecializationsAsync(int institutionId, int pageIndex, int pageSize, string filter)
        {
            return await this.MedicalSpecializationRepository.GetSpecializationsAsync(institutionId, pageIndex, pageSize, filter);
        }

        public async Task<int> GetSpecializationsCountAsync(int institutionId, string filter)
        {
            return await this.MedicalSpecializationRepository.GetSpecializationsCountAsync(institutionId, filter);
        }

        public async Task<MedicalSpecialization> GetSpecializationAsync(int specializationId)
        {
            return await this.MedicalSpecializationRepository.GetMedicalSpecializationAsync(specializationId);
        }
    }
}

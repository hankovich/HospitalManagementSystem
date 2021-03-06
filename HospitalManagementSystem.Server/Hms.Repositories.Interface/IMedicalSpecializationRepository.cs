﻿namespace Hms.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalSpecializationRepository
    {
        Task<MedicalSpecialization> GetMedicalSpecializationAsync(int id);

        Task<int> InsertOrUpdateMedicalSpecializationAsync(MedicalSpecialization specialization);

        Task<IEnumerable<MedicalSpecialization>> GetSpecializationsAsync(int institutionId, int pageIndex, int pageSize, string filter);

        Task<int> GetSpecializationsCountAsync(int institutionId, string filter);
    }
}
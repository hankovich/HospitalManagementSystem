﻿namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalCardRepository
    {
        Task<MedicalCard> GetMedicalCardPagesAsync(string login, int pageIndex, int pageSize = 20, string filter = "");

        Task<MedicalCardRecord> GetMedicalRecordAsync(string login, int recordId);
    }
}

﻿namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalCardService
    {
        Task<MedicalCard> GetMedicalCardPagesAsync(string login, int pageIndex, int pageSize = 20);
    }
}

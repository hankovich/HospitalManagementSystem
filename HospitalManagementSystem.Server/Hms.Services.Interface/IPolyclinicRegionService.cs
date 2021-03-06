﻿namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicRegionService
    {
        Task<PolyclinicRegion> GetRegionAsync(int id);

        Task<PolyclinicRegion> GetRegionAsync(int polyclinicId, int regionNumber);

        Task<int> InsertOrUpdateRegionAsync(PolyclinicRegion polyclinicRegion);
    }
}

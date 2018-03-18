﻿namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicRegionService
    {
        Task<PolyclinicRegion> GetPolyclinicRegionAsync(int polyclinicRegionId);

        Task<int> InsertOrUpdatePolyclinicRegionAsync(PolyclinicRegion polyclinicRegion);
    }
}

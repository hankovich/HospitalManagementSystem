namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class PolyclinicRegionService : IPolyclinicRegionService
    {
        public Task<PolyclinicRegion> GetRegionAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}

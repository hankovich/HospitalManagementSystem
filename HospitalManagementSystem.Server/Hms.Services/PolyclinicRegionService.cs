namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class PolyclinicRegionService : IPolyclinicRegionService
    {
        public PolyclinicRegionService(IPolyclinicRegionRepository regionRepository)
        {
            this.RegionRepository = regionRepository;
        }

        public IPolyclinicRegionRepository RegionRepository { get; }

        public async Task<PolyclinicRegion> GetRegionAsync(int id)
        {
            return await this.RegionRepository.GetRegionAsync(id);
        }

        public async Task<PolyclinicRegion> GetRegionAsync(int polyclinicId, int regionNumber)
        {
            return await this.RegionRepository.GetRegionAsync(polyclinicId, regionNumber);
        }

        public Task<int> InsertOrUpdateRegionAsync(PolyclinicRegion polyclinicRegion)
        {
            return this.InsertOrUpdateRegionAsync(polyclinicRegion);
        }
    }
}

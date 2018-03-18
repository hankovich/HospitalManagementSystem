namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class PolyclinicRegionService : IPolyclinicRegionService
    {
        public PolyclinicRegionService(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; set; }

        public Task<PolyclinicRegion> GetPolyclinicRegionAsync(int polyclinicRegionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> InsertOrUpdatePolyclinicRegionAsync(PolyclinicRegion polyclinicRegion)
        {
            throw new System.NotImplementedException();
        }
    }
}

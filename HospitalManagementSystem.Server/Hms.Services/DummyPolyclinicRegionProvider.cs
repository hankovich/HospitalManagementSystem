namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;

    public class DummyPolyclinicRegionProvider : IPolyclinicRegionProvider
    {
        public Task<int> GetPolyclinicRegionIdAsync(Address address)
        {
            return Task.FromResult(1);
        }
    }
}

namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Geocoding;

    public interface IPolyclinicRegionProvider
    {
        Task<int> GetPolyclinicRegionIdAsync(Address address);
    }
}

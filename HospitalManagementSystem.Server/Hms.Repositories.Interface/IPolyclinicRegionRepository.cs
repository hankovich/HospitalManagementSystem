namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicRegionRepository
    {
        Task<PolyclinicRegion> GetRegionAsync(int id);

        Task<PolyclinicRegion> GetRegionAsync(int polyclinicId, int regionNumber);

        Task<int> InsertOrUpdateRegionAsync(PolyclinicRegion polyclinicRegion);
    }
}

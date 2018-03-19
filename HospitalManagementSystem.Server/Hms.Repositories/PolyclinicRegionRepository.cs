namespace Hms.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class PolyclinicRegionRepository : IPolyclinicRegionRepository
    {
        public PolyclinicRegionRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public Task<PolyclinicRegion> GetRegionAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PolyclinicRegion> GetRegionAsync(int polyclinicId, int regionNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> InsertOrUpdateRegionAsync(PolyclinicRegion polyclinicRegion)
        {
            throw new System.NotImplementedException();
        }
    }
}

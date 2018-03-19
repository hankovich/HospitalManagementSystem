namespace Hms.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class BuildingRepository : IBuildingRepository
    {
        public BuildingRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public Task<BuildingAddress> GetBuildingAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> InsertOrUpdateBuildingAsync(BuildingAddress address)
        {
            throw new System.NotImplementedException();
        }
    }
}

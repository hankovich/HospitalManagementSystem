namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class BuildingService : IBuildingService
    {
        public BuildingService(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; set; }

        public Task<Profile> GetBuildingAsync(int buildingId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> InsertOrUpdateBuildingAsync(BuildingAddress buildingAddress)
        {
            throw new System.NotImplementedException();
        }
    }
}

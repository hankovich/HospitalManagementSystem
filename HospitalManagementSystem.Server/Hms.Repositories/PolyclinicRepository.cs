namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class PolyclinicRepository : IPolyclinicRepository
    {
        public PolyclinicRepository(string connectionString, IPolyclinicRegionRepository polyclinicRegionRepository, IBuildingRepository buildingRepository)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.PolyclinicRegionRepository = polyclinicRegionRepository;
            this.BuildingRepository = buildingRepository;
        }

        public string ConnectionString { get; }

        public IPolyclinicRegionRepository PolyclinicRegionRepository { get; }

        public IBuildingRepository BuildingRepository { get; }

        public async Task<Polyclinic> GetPolyclinicAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    SELECT [HeathcareInstitutionId] AS [Id], [Name], [Phone], [Address] AS [AddressId], PR.[Id]
                    FROM [Polyclinic] P
                    JOIN [PolyclinicRegion] PR
                    ON P.[Id] = PR.[PolyclinicId]

                    WHERE [HeathcareInstitutionId] = @id";

                    var polyclinicTasks = await connection.QueryAsync<Polyclinic, int, int, Task<Polyclinic>>(
                                              command,
                                              async (info, addressId, regionId) =>
                                              {
                                                  if (info.Address == null)
                                                  {
                                                      info.Address =
                                                          await this.BuildingRepository.GetBuildingAsync(addressId);
                                                  }

                                                  if (info.Regions == null)
                                                  {
                                                      info.Regions = new List<PolyclinicRegion>();
                                                  }

                                                  info.Regions.Add(await this.PolyclinicRegionRepository.GetRegionAsync(regionId));
                                                  return info;
                                              },
                                              new { id });

                    var polyclinics = await Task.WhenAll(polyclinicTasks);

                    var polyclinic = polyclinics.GroupBy(o => o.Id).Select(
                        group =>
                        {
                            var polyclinicTemplate = group.First();
                            polyclinicTemplate.Regions =
                                new List<PolyclinicRegion>(
                                    group.Select(gr => gr.Regions.FirstOrDefault()).Where(region => region != null));

                            return polyclinicTemplate;
                        }).FirstOrDefault();

                    return polyclinics.First();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdatePolyclinicAsync(Polyclinic polyclinic)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PolyclinicInfo : Polyclinic
    {
        public int AddressId { get; set; }

        public IEnumerable<int> RegionIds { get; set; }
    }
}

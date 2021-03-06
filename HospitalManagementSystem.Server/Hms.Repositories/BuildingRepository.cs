﻿namespace Hms.Repositories
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class BuildingRepository : IBuildingRepository
    {
        public BuildingRepository(string connectionString, IPolyclinicRegionRepository regionRepository)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.RegionRepository = regionRepository;
        }

        public string ConnectionString { get; }

        public IPolyclinicRegionRepository RegionRepository { get; }

        public async Task<BuildingAddress> GetBuildingAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"

                    IF NOT EXISTS (SELECT [Id] FROM [BuildingAddress] WHERE [Id] = @id) 
					    BEGIN
						    RAISERROR ('There is no such building', 16, 1)
                        END

                    SELECT 
                    [Id],
	                [City],
	                [Street],
	                [Building],
	                [Latitude],
	                [Longitude],
	                [PolyclinicRegionId]
	                FROM [BuildingAddress] BA
                    WHERE BA.[Id] = @id";

                    var address = await connection.QueryFirstOrDefaultAsync<BuildingAddressInfo>(command, new { id });

                    if (address.PolyclinicRegionId != default(int))
                    {
                        address.PolyclinicRegion =
                            await this.RegionRepository.GetRegionAsync(address.PolyclinicRegionId);
                    }

                    return address;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> GetBuildingIdOrDefaultAsync(double latitude, double longitude)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"

                    IF NOT EXISTS (SELECT [Id] FROM [BuildingAddress] WHERE [Latitude] = @latitude AND [Longitude] = @longitude) 
					    BEGIN
						    SELECT 0
                        END

                    SELECT 
                    [Id]
	                FROM [BuildingAddress]
                    WHERE [Latitude] = @latitude AND [Longitude] = @longitude";

                    var id = await connection.QueryFirstOrDefaultAsync<int>(command, new { latitude, longitude });

                    return id;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateBuildingAsync(BuildingAddress address)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    int regionId = await this.RegionRepository.InsertOrUpdateRegionAsync(address.PolyclinicRegion);

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT [Id] FROM [BuildingAddress] WHERE [Id] = @Id) 
		                    BEGIN
			                    UPDATE [BuildingAddress] WITH (SERIALIZABLE) 
						                SET 
	                                        [City] = @City, 
	                                        [Street] = @Street, 
	                                        [Building] = @Building,
	                                        [Latitude] = @Latitude,
	                                        [Longitude] = @Longitude, 
	                                        [PolyclinicRegionId] = @regionId
                                        WHERE [Id] = @Id
                                        SELECT @Id
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) OUTPUT INSERTED.ID VALUES 
                                                        (@City, @Street, @Building, @Latitude, @Longitude, @regionId)
		                    END
                    COMMIT TRAN";

                    var id = await connection.QueryFirstOrDefaultAsync<int>(command, new { address.Id, address.City, address.Street, address.Building, address.Latitude, address.Longitude, regionId });

                    return id;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }

    public class BuildingAddressInfo : BuildingAddress
    {
        public int PolyclinicRegionId { get; set; }
    }
}

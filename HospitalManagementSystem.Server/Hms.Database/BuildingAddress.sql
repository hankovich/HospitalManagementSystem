CREATE TABLE [dbo].[BuildingAddress]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[City] NVARCHAR (100) NOT NULL,
	[Street] NVARCHAR(100) NOT NULL,
	[Building] NVARCHAR(100) NOT NULL,
	[Latitude] FLOAT NOT NULL,
	[Longitude] FLOAT NOT NULL,
	[PolyclinicRegionId] INT NULL REFERENCES [PolyclinicRegion]([Id]) ON DELETE SET NULL
)

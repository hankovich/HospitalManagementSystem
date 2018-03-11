CREATE TABLE [dbo].[PolyclinicRegion]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PolyclinicId] INT NOT NULL REFERENCES [Polyclinic]([Id]) ON DELETE CASCADE,
	[RegionNumber] INT NOT NULL,
	[RegionHeadId] INT NULL REFERENCES [Doctor]([UserId]) ON DELETE SET NULL
)

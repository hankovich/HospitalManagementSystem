CREATE TABLE [dbo].[Hospital]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[HeathcareInstitutionId] INT NOT NULL REFERENCES [HealthcareInstitution]([Id]) ON DELETE CASCADE,
	[Name] NVARCHAR(100) NOT NULL,
	[Address] INT NULL REFERENCES [BuildingAddress]([Id]) ON DELETE SET NULL,
	[Phone] NVARCHAR(20) NULL
)

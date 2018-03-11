CREATE TABLE [dbo].[Nurse]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserId] INT NOT NULL REFERENCES [User]([Id]) ON DELETE CASCADE,
	[Info] NVARCHAR(MAX) NULL,
	[HealthcareInstitutionId] INT NULL REFERENCES [HealthcareInstitution]([Id]) ON DELETE SET NULL,
	[CabinetNumber] INT NULL
)

CREATE TABLE [dbo].[Nurse]
(
	[UserId] INT NOT NULL PRIMARY KEY REFERENCES [User]([Id]) ON DELETE CASCADE,
	[Info] NVARCHAR(MAX) NULL,
	[HealthcareInstitutionId] INT NULL REFERENCES [HealthcareInstitution]([Id]) ON DELETE SET NULL,
	[CabinetNumber] INT NULL
)

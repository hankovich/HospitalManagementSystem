﻿CREATE TABLE [dbo].[HospitalizationInfo]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserId] INT NOT NULL REFERENCES [User]([Id]) ON DELETE CASCADE,
	[StartDateUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[EndDateUtc] DATETIME NULL,
	[HospitalWardId] INT NULL REFERENCES [HospitalWard]([Id]) ON DELETE SET NULL
)

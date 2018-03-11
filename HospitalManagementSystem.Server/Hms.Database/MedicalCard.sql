﻿CREATE TABLE [dbo].[MedicalCard]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserId] INT NOT NULL REFERENCES [User]([Id]) ON DELETE CASCADE,
	[StartedAtUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE())
)

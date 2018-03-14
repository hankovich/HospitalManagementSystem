﻿CREATE TABLE [dbo].[MedicalCardRecord]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[MedicalCardId] INT NOT NULL REFERENCES [MedicalCard]([Id]) ON DELETE NO ACTION,
	[AssociatedRecordId] INT NULL REFERENCES [MedicalCardRecord]([Id]) ON DELETE NO ACTION,
	[DoctorId] INT NULL REFERENCES [Doctor]([UserId]) ON DELETE SET NULL,
	[AddedAtUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[ModifiedAtUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[Content] NVARCHAR(MAX) NOT NULL
)
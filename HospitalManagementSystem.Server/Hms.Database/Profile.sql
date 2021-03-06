﻿CREATE TABLE [dbo].[Profile]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UserId] INT NOT NULL REFERENCES [User]([Id]) ON DELETE CASCADE,
	[FirstName] NVARCHAR(100) NOT NULL,
	[MiddleName] NVARCHAR(100) NOT NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[DateOfBirth] DATETIME NOT NULL,
	[Phone] NVARCHAR(20) NULL,
	[BuildingId] INT NULL REFERENCES [BuildingAddress]([Id]) ON DELETE SET NULL,
	[Entrance] INT NULL,
	[Floor] INT NULL,
	[Flat] INT NULL,
	[Photo] VARBINARY(MAX) NULL
)

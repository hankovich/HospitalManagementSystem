CREATE TABLE [dbo].[HospitalWard]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[HospitalDepartmentId] INT NULL REFERENCES [HospitalDepartment]([Id]) ON DELETE SET NULL,
	[WardNumber] INT NOT NULL,
	[CotsCount] INT NOT NULL
)

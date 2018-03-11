CREATE TABLE [dbo].[MedicalSpecialization]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(MAX) NULL
)

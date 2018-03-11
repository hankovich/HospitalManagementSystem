CREATE TABLE [dbo].[HospitalDepartment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[HospitalId] INT NOT NULL REFERENCES [Hospital]([Id]) ON DELETE CASCADE,
	[MedicalSpecializationId] INT NOT NULL REFERENCES [MedicalSpecialization]([Id]) ON DELETE NO ACTION,
	[DepartmentHeadId] INT NOT NULL REFERENCES [Doctor]([UserId]) ON DELETE NO ACTION,
	[NurseHeadId] INT NOT NULL REFERENCES [Nurse]([UserId]) ON DELETE NO ACTION
)

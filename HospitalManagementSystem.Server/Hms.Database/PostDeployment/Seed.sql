/*
Шаблон скрипта после развертывания							
--------------------------------------------------------------------------------------
 В данном файле содержатся инструкции SQL, которые будут добавлены в скрипт построения.		
 Используйте синтаксис SQLCMD для включения файла в скрипт после развертывания.			
 Пример:      :r .\myfile.sql								
 Используйте синтаксис SQLCMD для создания ссылки на переменную в скрипте после развертывания.		
 Пример:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO [Role] VALUES ('Patient')
INSERT INTO [Role] VALUES ('Doctor')
INSERT INTO [Role] VALUES ('Nurse')
INSERT INTO [Role] VALUES ('Admin')

INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor1', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor1 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor2', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor2 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor3', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor3 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor4', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor4 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor5', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor5 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor6', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor6 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor7', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor7 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor8', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor8 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor9', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor9 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor10', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor10 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor11', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor11 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor12', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor12 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor13', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor13 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor14', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor14 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor15', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor15 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor16', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor16 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor17', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor17 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor18', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor18 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor19', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor19 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor20', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor20 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor21', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor21 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor22', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor22 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor23', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor23 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor24', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor24 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('doctor25', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- doctor25 password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('user', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- user password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('testnurse', 'AABDAP0IzHQVUAr6E7DWobTUmIKkTmXx5IGVQ2gs3CBV8H6yweSR3QBgh9QDCKGOTQ==')  -- testnurse password

INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 1)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 2)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 3)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 4)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 5)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 6)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 7)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 8)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 9)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 10)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 11)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 12)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 13)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 14)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 15)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 16)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 17)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 18)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 19)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 20)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 21)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 22)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 23)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 24)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 25)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 26)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 27)

INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 1)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 2)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 3)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 4)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 5)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 6)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 7)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 8)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 9)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 10)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 11)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 12)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 13)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 14)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 15)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 16)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 17)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 18)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 19)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 20)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 21)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 22)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 23)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 24)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 25)

INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (3, 27)

INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Аллергология', N'Описание аллергологии')
INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Травмотология', N'Описание травмотологии')
INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Отоларингология', N'Описание отоларингологии')
INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Проктология', N'Описание проктологии')
INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Урология', N'Описание урология')
INSERT INTO [MedicalSpecialization] ([Name], [Description]) VALUES (N'Терапия', N'Описание терапии')

INSERT INTO [HealthcareInstitution] ([Name]) VALUES (N'1-я городская поликлиника г. Минска')
INSERT INTO [HealthcareInstitution] ([Name]) VALUES (N'2-я городская поликлиника г. Минска')
INSERT INTO [HealthcareInstitution] ([Name]) VALUES (N'3-я городская поликлиника г. Минска')
INSERT INTO [HealthcareInstitution] ([Name]) VALUES (N'4-я городская поликлиника г. Минска')
INSERT INTO [HealthcareInstitution] ([Name]) VALUES (N'5-я городская поликлиника г. Минска')

INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) VALUES (N'Минск, Минск, Беларусь', N'Сухая улица', N'2', 53.904724, 27.539724, NULL)
INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) VALUES (N'Минск, Минск, Беларусь', N'улица Якубовского', N'32А', 53.897599, 27.452057, NULL)
INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) VALUES (N'Минск, Минск, Беларусь', N'улица Воронянского', N'13к2', 53.878893, 27.54417, NULL)
INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) VALUES (N'Минск, Минск, Беларусь', N'улица Сергея Есенина', N'20', 53.850247, 27.459926, NULL)
INSERT INTO [BuildingAddress] ([City], [Street], [Building], [Latitude], [Longitude], [PolyclinicRegionId]) VALUES (N'Минск, Минск, Беларусь', N'Ульяновская улица', N'22', 53.89561, 27.56458, NULL)

INSERT INTO [Polyclinic] ([HeathcareInstitutionId], [Name], [Address], [Phone]) VALUES (1, N'1-я городская поликлиника г. Минска', 1, '222-22-21')
INSERT INTO [Polyclinic] ([HeathcareInstitutionId], [Name], [Address], [Phone]) VALUES (2, N'2-я городская поликлиника г. Минска', 2, '222-22-22')
INSERT INTO [Polyclinic] ([HeathcareInstitutionId], [Name], [Address], [Phone]) VALUES (3, N'3-я городская поликлиника г. Минска', 3, '222-22-23')
INSERT INTO [Polyclinic] ([HeathcareInstitutionId], [Name], [Address], [Phone]) VALUES (4, N'4-я городская поликлиника г. Минска', 4, '222-22-24')
INSERT INTO [Polyclinic] ([HeathcareInstitutionId], [Name], [Address], [Phone]) VALUES (5, N'5-я городская поликлиника г. Минска', 5, '222-22-25')

INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (1, N'Работаю уже 3 лет', 1, 80, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (2, N'Работаю уже 15 лет', 1, 154, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (3, N'Работаю уже 4 лет', 1, 143, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (4, N'Работаю уже 24 лет', 1, 290, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (5, N'Работаю уже 19 лет', 1, 333, 5)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (6, N'Работаю уже 5 лет', 2, 451, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (7, N'Работаю уже 6 лет', 2, 34, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (8, N'Работаю уже 8 лет', 2, 363, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (9, N'Работаю уже 24 лет', 2, 240, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (10, N'Работаю уже 8 лет', 2, 274, 5)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (11, N'Работаю уже 1 лет', 3, 230, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (12, N'Работаю уже 11 лет', 3, 475, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (13, N'Работаю уже 3 лет', 3, 392, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (14, N'Работаю уже 7 лет', 3, 157, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (15, N'Работаю уже 5 лет', 3, 442, 5)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (16, N'Работаю уже 4 лет', 4, 62, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (17, N'Работаю уже 12 лет', 4, 492, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (18, N'Работаю уже 14 лет', 4, 168, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (19, N'Работаю уже 13 лет', 4, 228, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (20, N'Работаю уже 10 лет', 4, 395, 5)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (21, N'Работаю уже 25 лет', 5, 281, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (22, N'Работаю уже 13 лет', 5, 396, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (23, N'Работаю уже 8 лет', 5, 197, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (24, N'Работаю уже 2 лет', 5, 426, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (25, N'Работаю уже 22 лет', 5, 431, 5)

INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (1, 1, 1)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (1, 2, 2)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (1, 3, 3)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (1, 4, 4)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (1, 5, 5)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (2, 1, 6)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (2, 2, 7)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (2, 3, 8)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (2, 4, 9)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (2, 5, 10)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (3, 1, 11)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (3, 2, 12)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (3, 3, 13)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (3, 4, 14)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (3, 5, 15)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (4, 1, 16)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (4, 2, 17)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (4, 3, 18)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (4, 4, 19)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (4, 5, 20)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (5, 1, 21)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (5, 2, 22)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (5, 3, 23)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (5, 4, 24)
INSERT INTO [PolyclinicRegion] ([PolyclinicId], [RegionNumber], [RegionHeadId]) VALUES (5, 5, 25)

INSERT INTO [MedicalCard] ([UserId]) VALUES (1)
INSERT INTO [MedicalCard] ([UserId]) VALUES (2)
INSERT INTO [MedicalCard] ([UserId]) VALUES (3)
INSERT INTO [MedicalCard] ([UserId]) VALUES (4)
INSERT INTO [MedicalCard] ([UserId]) VALUES (5)
INSERT INTO [MedicalCard] ([UserId]) VALUES (6)

INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (1, NULL, 1, N'Здоров 1')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (2, 1, 1, N'Здоров 2')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (3, NULL, 1, N'Здоров 3')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (4, 2, 1, N'Здоров 4')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (5, NULL, 1, N'Здоров 5')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (6, 3, 1, N'Здоров 6')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (1, NULL, 2, N'Здоров 7')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (2, 4, 2, N'Здоров 8')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (3, NULL, 2, N'Здоров 9')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (4, 5, 2, N'Здоров 10')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (5, NULL, 2, N'Здоров 11')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (6, 6, 2, N'Здоров 12')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (1, NULL, 3, N'Здоров 13')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (2, 7, 3, N'Здоров 14')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (3, NULL, 3, N'Здоров 15')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (4, 8, 3, N'Здоров 16')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (5, NULL, 3, N'Здоров 17')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (6, 9, 3, N'Здоров 18')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (1, NULL, 4, N'Здоров 19')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (2, 10, 4, N'Здоров 20')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (3, NULL, 4, N'Здоров 21')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (4, 11, 4, N'Здоров 22')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (5, NULL, 4, N'Здоров 23')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (6, 12, 4, N'Здоров 24')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (1, NULL, 5, N'Здоров 25')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (2, 13, 5, N'Здоров 26')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (3, NULL, 5, N'Здоров 27')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (4, 14, 5, N'Здоров 28')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (5, NULL, 5, N'Здоров 29')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecordId], [DoctorId], [Content]) VALUES (6, 15, 5, N'Здоров 30')
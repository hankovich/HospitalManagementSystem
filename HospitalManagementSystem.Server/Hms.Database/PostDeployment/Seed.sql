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
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('user', 'AJJ32/nAuUJ153+3UzLrMXk0UYcNMbWHii/3ufTkBPTsldiz1YRjO4GqZznW/vKI0w==') -- user password
INSERT INTO [User] ([Login], [PasswordHash]) VALUES ('testnurse', 'AABDAP0IzHQVUAr6E7DWobTUmIKkTmXx5IGVQ2gs3CBV8H6yweSR3QBgh9QDCKGOTQ==')  -- testnurse password

INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 1)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 2)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 3)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 4)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 5)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 6)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (1, 7)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 1)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 2)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 3)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 4)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (2, 5)
INSERT INTO [UserRole] ([RoleId], [UserId]) VALUES (3, 7)

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

INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (1, N'Работаю уже 5 лет', 1, 103, 1)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (2, N'Работаю уже 15 лет', 2, 203, 2)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (3, N'Работаю уже 25 лет', 3, 303, 3)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (4, N'Работаю уже 35 лет', 4, 403, 4)
INSERT INTO [Doctor] ([UserId], [Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) VALUES (5, N'Работаю уже 45 лет', 5, 503, 5)

INSERT INTO [MedicalCard] ([UserId]) VALUES (1)
INSERT INTO [MedicalCard] ([UserId]) VALUES (2)
INSERT INTO [MedicalCard] ([UserId]) VALUES (3)
INSERT INTO [MedicalCard] ([UserId]) VALUES (4)
INSERT INTO [MedicalCard] ([UserId]) VALUES (5)
INSERT INTO [MedicalCard] ([UserId]) VALUES (6)

INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (1, NULL, 1, N'Здоров 1')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (2, NULL, 1, N'Здоров 2')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (3, NULL, 1, N'Здоров 3')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (4, NULL, 1, N'Здоров 4')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (5, NULL, 1, N'Здоров 5')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (6, NULL, 1, N'Здоров 6')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (1, NULL, 2, N'Здоров 7')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (2, NULL, 2, N'Здоров 8')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (3, NULL, 2, N'Здоров 9')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (4, NULL, 2, N'Здоров 10')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (5, NULL, 2, N'Здоров 11')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (6, NULL, 2, N'Здоров 12')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (1, NULL, 3, N'Здоров 13')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (2, NULL, 3, N'Здоров 14')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (3, NULL, 3, N'Здоров 15')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (4, NULL, 3, N'Здоров 16')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (5, NULL, 3, N'Здоров 17')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (6, NULL, 3, N'Здоров 18')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (1, NULL, 4, N'Здоров 19')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (2, NULL, 4, N'Здоров 20')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (3, NULL, 4, N'Здоров 21')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (4, NULL, 4, N'Здоров 22')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (5, NULL, 4, N'Здоров 23')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (6, NULL, 4, N'Здоров 24')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (1, NULL, 5, N'Здоров 25')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (2, NULL, 5, N'Здоров 26')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (3, NULL, 5, N'Здоров 27')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (4, NULL, 5, N'Здоров 28')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (5, NULL, 5, N'Здоров 29')
INSERT INTO [MedicalCardRecord] ([MedicalCardId], [AssociatedRecord], [DoctorId], [Content]) VALUES (6, NULL, 5, N'Здоров 30')

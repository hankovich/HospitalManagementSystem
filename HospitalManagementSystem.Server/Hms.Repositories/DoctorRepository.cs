namespace Hms.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class DoctorRepository : IDoctorRepository
    {
        public DoctorRepository(string connectionString, IHealthcareInstitutionRepository healthcareInstitutionRepository, IMedicalSpecializationRepository medicalSpecializationRepository)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
            this.HealthcareInstitutionRepository = healthcareInstitutionRepository;
            this.MedicalSpecializationRepository = medicalSpecializationRepository;
        }

        public string ConnectionString { get; set; }

        public IHealthcareInstitutionRepository HealthcareInstitutionRepository { get; }

        public IMedicalSpecializationRepository MedicalSpecializationRepository { get; }

        public async Task<Doctor> GetDoctorAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT [UserId] FROM [Doctor] WHERE [UserId] = @id) 
		                    BEGIN
				                SELECT 
								UD.[Id], UD.[Login], NULL AS PasswordHash, D.[Info], D.[CabinetNumber], HI.[Id], HI.[Name], MS.[Id], MS.[Name], MS.[Description] 
								
								FROM [Doctor] D 
								LEFT JOIN 
								[User] UD 
								ON 
								UD.[Id] = D.[UserId]
								LEFT JOIN [HealthcareInstitution] HI
								ON D.[HealthcareInstitutionId] = HI.[Id]
								LEFT JOIN [MedicalSpecialization] MS 
								ON D.[MedicalSpecializationId] = MS.[Id]
								WHERE U.[Id] = @id
		                    END
	                    ELSE
		                    BEGIN
			                    RAISERROR ('There is no such doctor', 16, 1)
		                    END
                    COMMIT TRAN";

                    IEnumerable<Doctor> doctors =
                        await
                        connection.QueryAsync<Doctor, HealthcareInstitution, MedicalSpecialization, Doctor>(
                            command,
                            (doctor, institution, specialization) =>
                            {
                                doctor.Institution = institution;
                                doctor.Specialization = specialization;

                                return doctor;
                            },
                            new { id });

                    return doctors.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<int> InsertOrUpdateDoctorAsync(Doctor doctor)
        {
            try
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    int healthcareInstitutionId =
                        await this.HealthcareInstitutionRepository.InsertOrUpdateHealthcareInstitutionAsync(
                            doctor.Institution);

                    int medicalSpecializationId =
                        await this.MedicalSpecializationRepository.InsertOrUpdateMedicalSpecializationAsync(
                            doctor.Specialization);

                    var command = @"
                    BEGIN TRAN
	                    IF EXISTS (SELECT * FROM [Doctor] WHERE [UserId] = @Id) 
		                    BEGIN
                                UPDATE [Doctor] WITH (SERIALIZABLE) 
						            SET 
                                        [Info] = @Info,
                                        [HealthcareInstitutionId] = @healthcareInstitutionId,
                                        [CabinetNumber] = @CabinetNumber,
                                        [MedicalSpecializationId] = @medicalSpecializationId
	                                    WHERE [UserId] = @Id
                                        SELECT @Id
		                    END
	                    ELSE
		                    BEGIN
			                    INSERT INTO [Doctor] ([Info], [HealthcareInstitutionId], [CabinetNumber], [MedicalSpecializationId]) OUTPUT INSERTED.UserId VALUES 
                                                        (@Info, @healthcareInstitutionId, @CabinetNumber, @medicalSpecializationId)
		                    END
                    COMMIT TRAN";

                    return await connection.ExecuteAsync(command, new { doctor.Id, doctor.Info, doctor.CabinetNumber, healthcareInstitutionId, medicalSpecializationId });
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}

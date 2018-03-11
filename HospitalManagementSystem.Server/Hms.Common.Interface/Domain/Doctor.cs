namespace Hms.Common.Interface.Domain
{
    public class Doctor : User
    {
        public string Info { get; set; }

        public HealthcareInstitution Institution { get; set; }

        public int CabinetNumber { get; set; }

        public MedicalSpecialization Specialization { get; set; }
    }
}
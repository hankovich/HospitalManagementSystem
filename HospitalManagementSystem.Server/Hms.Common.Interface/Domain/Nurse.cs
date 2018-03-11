namespace Hms.Common.Interface.Domain
{
    public class Nurse : User
    {
        public string Info { get; set; }

        public HealthcareInstitution Institution { get; set; }

        public int CabinetNumber { get; set; }
    }
}
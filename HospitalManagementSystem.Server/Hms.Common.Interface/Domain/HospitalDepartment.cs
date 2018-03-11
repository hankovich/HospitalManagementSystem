namespace Hms.Common.Interface.Domain
{
    public class HospitalDepartment
    {
        public int Id { get; set; }

        public MedicalSpecialization Specialization { get; set; }

        public Doctor DepartmentHead { get; set; }

        public Nurse NurseHead { get; set; }
    }
}
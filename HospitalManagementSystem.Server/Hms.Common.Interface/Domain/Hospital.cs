namespace Hms.Common.Interface.Domain
{
    using System.Collections.Generic;

    public class Hospital : HealthcareInstitution
    {
        public BuildingAddress Address { get; set; }

        public string Phone { get; set; }

        public ICollection<HospitalDepartment> Departments { get; set; }
    }
}
namespace Hms.Common.Interface.Domain
{
    using System.Collections.Generic;

    public class Hospital : HealthcareInstitution
    {
        public string Name { get; set; }

        public BuildingAddress Address { get; set; }

        public string Phone { get; set; }

        public ICollection<HospitalDepartment> Departments { get; set; }
    }
}
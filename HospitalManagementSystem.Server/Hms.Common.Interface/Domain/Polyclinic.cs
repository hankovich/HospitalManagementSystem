﻿namespace Hms.Common.Interface.Domain
{
    using System.Collections.Generic;

    public class Polyclinic : HealthcareInstitution
    {
        public string Name { get; set; }

        public BuildingAddress Address { get; set; }

        public string Phone { get; set; }

        public ICollection<PolyclinicRegion> Regions { get; set; }
    }
}
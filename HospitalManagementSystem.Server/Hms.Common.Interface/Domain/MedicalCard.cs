namespace Hms.Common.Interface.Domain
{
    using System;
    using System.Collections.Generic;

    public class MedicalCard
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime StartedAtUtc { get; set; }

        public int TotalRecords { get; set; }

        public ICollection<MedicalCardRecord> Records { get; set; }
    }
}
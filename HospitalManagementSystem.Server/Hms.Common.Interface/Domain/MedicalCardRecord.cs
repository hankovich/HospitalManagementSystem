namespace Hms.Common.Interface.Domain
{
    using System;

    public class MedicalCardRecord
    {
        public int Id { get; set; }

        public MedicalCardRecord AssociatedRecord { get; set; }

        public Doctor Author { get; set; }

        public DateTime AddedAtUtc { get; set; }

        public DateTime ModifiedAtUtc { get; set; }

        public string Content { get; set; }
    }
}
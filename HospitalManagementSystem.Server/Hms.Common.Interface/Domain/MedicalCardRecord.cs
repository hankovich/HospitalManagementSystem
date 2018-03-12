namespace Hms.Common.Interface.Domain
{
    using System;
    using System.Collections.Generic;

    public class MedicalCardRecord
    {
        public int Id { get; set; }

        public int? AssociatedRecordId { get; set; }

        public Doctor Author { get; set; }

        public DateTime AddedAtUtc { get; set; }

        public DateTime ModifiedAtUtc { get; set; }

        public string Content { get; set; }

        public ICollection<int> AttachmentIds { get; set; } 
    }
}
namespace Hms.Common.Interface.Domain
{
    using System;

    public class AttachmentInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
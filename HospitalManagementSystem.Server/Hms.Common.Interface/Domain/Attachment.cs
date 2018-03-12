namespace Hms.Common.Interface.Domain
{
    using System;

    public class Attachment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
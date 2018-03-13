namespace Hms.Common.Interface.Domain
{
    using System;

    public class Profile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public byte[] Photo { get; set; }

        public string Phone { get; set; }

        public int? BuildingId { get; set; }

        public int? Entrance { get; set; }

        public int? Floor { get; set; }

        public int? Flat { get; set; }
    }
}

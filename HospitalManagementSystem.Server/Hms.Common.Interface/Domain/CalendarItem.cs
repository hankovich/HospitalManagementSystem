namespace Hms.Common.Interface.Domain
{
    using System;
    using System.Collections.Generic;

    public class CalendarItem
    {
        public int Id { get; set; }

        public User Owner { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Info { get; set; } 

        public ICollection<User> AssociatedUsers { get; set; } 
    }
}

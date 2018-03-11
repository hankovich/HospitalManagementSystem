namespace Hms.Services.Interface.Models
{
    using System;

    [Flags]
    public enum Role
    {
        Patient = 1,
        Doctor = 2,
        Nurse = 4,
        Admin = 8
    }
}
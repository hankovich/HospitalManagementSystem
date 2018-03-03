namespace Hms.Services.Interface.Models
{
    using System;

    [Flags]
    public enum Role
    {
        Patient = 1,
        Doctor = 2
    }
}
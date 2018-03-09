namespace Hms.Services.Interface.Models
{
    using System;

    public class RoundKeyExpirationSettings
    {
        public int Requests { get; set; }

        public TimeSpan Time { get; set; }
    }
}
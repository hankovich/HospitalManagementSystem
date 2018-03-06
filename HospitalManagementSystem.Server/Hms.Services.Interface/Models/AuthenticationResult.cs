namespace Hms.Services.Interface.Models
{
    using Hms.Common.Interface.Models;

    public class AuthenticationResult
    {
        public PrincipalModel Principal { get; set; }

        public byte[] RoundKey { get; set; }

        public bool IsAuthenticated { get; set; }

        public string FailureReason { get; set; }

        public bool IsRoundKeyExpired { get; set; }
    }
}
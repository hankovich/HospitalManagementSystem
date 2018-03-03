namespace Hms.Services.Interface.Models
{
    using Hms.Common;

    public class AuthenticationResult
    {
        public PrincipalModel Principal { get; set; }

        public bool IsAuthenticated { get; set; }

        public string FailureReason { get; set; }

        public bool IsRoundKeyExpired { get; set; }
    }
}
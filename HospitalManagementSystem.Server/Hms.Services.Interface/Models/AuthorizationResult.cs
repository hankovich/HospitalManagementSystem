namespace Hms.Services.Interface.Models
{
    public class AuthorizationResult
    {
        public bool IsAuthorized { get; set; }

        public string[] AllRoles { get; set; }
    }
}
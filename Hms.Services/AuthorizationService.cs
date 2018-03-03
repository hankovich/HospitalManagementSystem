namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    public class AuthorizationService : IAuthorizationService
    {
        public async Task<AuthorizationResult> AuthorizeAsync(string login, Role roles)
        {
            var a = roles.GetFlags();
            return new AuthorizationResult();
        }
    }
}

namespace Hms.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Extensions;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    public class AuthorizationService : IAuthorizationService
    {
        public IUserRoleRepository UserRoleRepository { get; }

        public AuthorizationService(IUserRoleRepository userRoleRepository)
        {
            this.UserRoleRepository = userRoleRepository;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(string login, Role roles)
        {
            IEnumerable<string> allowed = roles.GetFlags();

            IEnumerable<string> actual = await this.UserRoleRepository.GetUserRolesAsync(login);

            bool isAuthorized = actual.Any(role => allowed.Contains(role));

            return new AuthorizationResult
            {
                IsAuthorized = isAuthorized
            };
        }
    }
}

namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Services.Interface.Models;

    public interface IAuthorizationService
    {
        Task<AuthorizationResult> AuthorizeAsync(string login, Role roles);
    }
}
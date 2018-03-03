namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Services.Interface.Models;

    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(string authenticationToken);
    }
}
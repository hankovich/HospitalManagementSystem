namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Services.Interface.Models;

    public interface IAuthenticationService
    {
        RoundKeyExpirationSettings KeyExpirationSettings { get; set; }

        Task<AuthenticationResult> AuthenticateAsync(string authenticationToken);
    }
}
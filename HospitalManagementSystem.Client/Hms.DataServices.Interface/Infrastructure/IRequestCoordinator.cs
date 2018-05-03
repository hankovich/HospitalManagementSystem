namespace Hms.DataServices.Interface.Infrastructure
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRequestCoordinator
    {
        string Host { get; }

        int? UserId { get; }

        Task<ServerResponse<T>> SendAsync<T>(HttpMethod method, string url, object content = null, bool needsEncryption = true, CancellationToken cancellationToken = default(CancellationToken));

        Task LoginAsync(string username, string password);

        Task RegisterAsync(string username, string password);

        Task LogoutAsync();

        Task ChangeAsymmetricKey();

        Task ChangeRoundKey();
    }
}
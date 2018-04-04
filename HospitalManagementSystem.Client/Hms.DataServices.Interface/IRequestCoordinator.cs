namespace Hms.DataServices.Interface
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IRequestCoordinator
    {
        string Host { get; }

        int? UserId { get; }

        Task<ServerResponse<T>> SendAsync<T>(HttpMethod method, string url, object content = null, bool needsEncryption = true);

        Task LoginAsync(string username, string password);

        Task RegisterAsync(string username, string password);

        Task LogoutAsync();

        Task ChangeAsymmetricKey();

        Task ChangeRoundKey();
    }
}
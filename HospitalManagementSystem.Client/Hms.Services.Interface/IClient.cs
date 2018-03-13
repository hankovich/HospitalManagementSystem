namespace Hms.Services.Interface
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IClient
    {
        string Host { get; }

        int? UserId { get; }

        Task<ServerResponse<T>> SendAsync<T>(HttpMethod method, string url, object content, bool needsEncryption = true);

        Task LoginAsync(string username, string password);

        Task RegisterAsync(string username, string password);

        Task LogoutAsync();

        Task ChangeAsymmetricKey();

        Task ChangeRoundKey();
    }
}
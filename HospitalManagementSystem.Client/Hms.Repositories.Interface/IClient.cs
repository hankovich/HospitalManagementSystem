namespace Hms.Repositories.Interface
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IClient
    {
        string Host { get; }

        Task<ServerResponse> SendAsync(HttpMethod method, string url, object content);

        Task LoginAsync(string username, string password);

        Task RegisterAsync(string username, string password);

        Task LogoutAsync();
    }
}

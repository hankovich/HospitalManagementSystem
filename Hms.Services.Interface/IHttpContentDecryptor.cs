namespace Hms.Services.Interface
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpContentDecryptor
    {
        Task<HttpContent> DecryptAsync(HttpContent originalContent, byte[] key);
    }
}

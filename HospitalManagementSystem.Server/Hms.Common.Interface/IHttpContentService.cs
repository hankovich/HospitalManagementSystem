namespace Hms.Common.Interface
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpContentService
    {
        Task<HttpContent> DecryptAsync(HttpContent originalContent, byte[] key);

        Task<HttpContent> EncryptAsync(HttpContent originalContent, byte[] key);
    }
}

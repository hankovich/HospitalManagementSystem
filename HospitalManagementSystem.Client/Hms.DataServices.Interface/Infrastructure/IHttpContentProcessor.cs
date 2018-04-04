namespace Hms.DataServices.Interface.Infrastructure
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpContentProcessor
    {
        Task<HttpContent> SerializeAsync(object content, byte[] roundKey, bool needsEncryption);

        Task<string> DeserializeAsync(HttpContent content, byte[] roundKey, bool needsDecryption);
    }
}

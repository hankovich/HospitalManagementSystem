namespace Hms.DataServices.Infrasructure
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    using Newtonsoft.Json;

    public class HttpContentProcessor : IHttpContentProcessor
    {
        public IHttpContentService ContentService { get; }

        public HttpContentProcessor(IHttpContentService contentService)
        {
            this.ContentService = contentService;
        }

        public async Task<HttpContent> SerializeAsync(object content, byte[] roundKey, bool needsEncryption)
        {
            HttpContent httpContent = ConvertToHttpContent(content);

            if (needsEncryption && httpContent != null)
            {
                return await this.ContentService.EncryptAsync(httpContent, roundKey);
            }

            return httpContent;
        }

        public async Task<string> DeserializeAsync(HttpContent content, byte[] roundKey, bool needsDecryption)
        {
            if (content == null || (content.Headers.ContentLength ?? 0) == 0)
            {
                return null;
            }

            if (needsDecryption)
            {
                content = await this.ContentService.DecryptAsync(content, roundKey);
            }

            return await content.ReadAsStringAsync();
        }

        private static HttpContent ConvertToHttpContent(object content)
        {
            if (content == null)
            {
                return null;
            }

            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
    }
}

namespace Hms.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    
    public class HttpContentService : IHttpContentService
    {
        public HttpContentService(ISymmetricCryptoProvider cryptoProvider)
        {
            if (cryptoProvider == null)
            {
                throw new ArgumentNullException(nameof(cryptoProvider));
            }

            this.CryptoProvider = cryptoProvider;
        }

        public ISymmetricCryptoProvider CryptoProvider { get; set; }

        public async Task<HttpContent> DecryptAsync(HttpContent originalContent, byte[] key)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = await originalContent.ReadAsByteArrayAsync();
            byte[] decryptedBytes = await this.CryptoProvider.DecryptBytesAsync(bytes, key, Enumerable.Repeat((byte)0, key.Length / 2).ToArray());

            return new StringContent(encoding.GetString(decryptedBytes), encoding, "application/json");
        }

        public async Task<HttpContent> EncryptAsync(HttpContent originalContent, byte[] key)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = await originalContent.ReadAsByteArrayAsync();
            byte[] decryptedBytes = await this.CryptoProvider.EncryptBytesAsync(bytes, key, Enumerable.Repeat((byte)0, key.Length / 2).ToArray());

            return new StringContent(encoding.GetString(decryptedBytes), encoding, "application/json");
        }
    }
}
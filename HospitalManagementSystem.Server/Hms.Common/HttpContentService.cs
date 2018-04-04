namespace Hms.Common
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

        public ISymmetricCryptoProvider CryptoProvider { get; }

        public async Task<HttpContent> DecryptAsync(HttpContent originalContent, byte[] key)
        {
            Encoding encoding = Encoding.UTF8;
            var contentString = await originalContent.ReadAsStringAsync();
            byte[] bytes = Convert.FromBase64String(contentString);
            byte[] decryptedBytes = await this.CryptoProvider.DecryptBytesAsync(bytes, key, Enumerable.Repeat((byte)0, key.Length / 2).ToArray());

            return new StringContent(encoding.GetString(decryptedBytes), encoding, "application/json");
        }

        public async Task<HttpContent> EncryptAsync(HttpContent originalContent, byte[] key)
        {
            Encoding encoding = Encoding.UTF8;
            var contentString = await originalContent.ReadAsStringAsync();
            byte[] bytes = encoding.GetBytes(contentString);
            byte[] encryptedBytes = await this.CryptoProvider.EncryptBytesAsync(bytes, key, Enumerable.Repeat((byte)0, key.Length / 2).ToArray());

            return new StringContent(Convert.ToBase64String(encryptedBytes), encoding, "application/json");
        }
    }
}
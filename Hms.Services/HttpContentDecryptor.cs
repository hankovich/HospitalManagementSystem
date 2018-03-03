namespace Hms.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common;
    using Hms.Services.Interface;

    public class HttpContentDecryptor : IHttpContentDecryptor
    {
        public HttpContentDecryptor(ISymmetricCryptoProvider cryptoProvider)
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
            byte[] decryptedBytes = await this.CryptoProvider.DecryptBytesAsync(bytes, key, bytes);

            return new StringContent(encoding.GetString(decryptedBytes), encoding, "application/json");
        }
    }
}
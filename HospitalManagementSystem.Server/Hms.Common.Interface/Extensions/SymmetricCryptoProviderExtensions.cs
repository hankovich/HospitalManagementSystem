namespace Hms.Common.Interface.Extensions
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public static class SymmetricCryptoProviderExtensions
    {
        public static async Task<string> EncryptBase64StringAsync(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (message == null)
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.EncryptBytesAsync(Convert.FromBase64String(message), key, iv));
        }

        public static async Task<string> EncryptUtf8StringAsync(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (message == null)
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.EncryptBytesAsync(Encoding.UTF8.GetBytes(message), key, iv));
        }

        public static async Task<string> DecryptBase64StringAsync(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            return Convert.ToBase64String(await provider.DecryptBytesAsync(Convert.FromBase64String(message), key, iv));
        }

        public static async Task<string> DecryptUtf8StringAsync(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            return Convert.ToBase64String(await provider.DecryptBytesAsync(Encoding.UTF8.GetBytes(message), key, iv));
        }
    }
}
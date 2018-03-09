namespace Hms.Common.Interface.Extensions
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public static class SymmetricCryptoProviderExtensions
    {
        public static async Task<string> EncryptBase64ToBase64Async(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.EncryptBytesAsync(Convert.FromBase64String(message), key, iv));
        }

        public static async Task<string> EncryptUtf8ToBase64Async(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.EncryptBytesAsync(Encoding.UTF8.GetBytes(message), key, iv));
        }

        public static async Task<string> DecryptBase64ToBase64Async(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.DecryptBytesAsync(Convert.FromBase64String(message), key, iv));
        }

        public static async Task<string> DecryptBase64ToUtf8Async(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(await provider.DecryptBytesAsync(Convert.FromBase64String(message), key, iv));
        }

        public static async Task<string> DecryptUtf8ToBase64Async(
            this ISymmetricCryptoProvider provider,
            string message,
            byte[] key,
            byte[] iv)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(await provider.DecryptBytesAsync(Encoding.UTF8.GetBytes(message), key, iv));
        }
    }
}
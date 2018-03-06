namespace Hms.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using Hms.Common.Interface;

    public class AesCryptoProvider : ISymmetricCryptoProvider
    {
        public int KeySize => 256;

        public async Task<byte[]> EncryptBytesAsync(byte[] message, byte[] key, byte[] iv)
        {
            if (message == null || message.Length <= 0)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream container = new MemoryStream())
                using (CryptoStream encryptedStream = new CryptoStream(container, encryptor, CryptoStreamMode.Write))
                {
                    var lengthBytes = BitConverter.GetBytes(message.Length);
                    await encryptedStream.WriteAsync(lengthBytes, 0, lengthBytes.Length);

                    await encryptedStream.WriteAsync(message, 0, message.Length);
                    encryptedStream.FlushFinalBlock();

                    encrypted = container.ToArray();
                }
            }

            return encrypted;
        }

        public async Task<byte[]> DecryptBytesAsync(byte[] message, byte[] key, byte[] iv)
        {
            if (message == null || message.Length <= 0)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            byte[] decrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream container = new MemoryStream(message))
                using (CryptoStream decryptedStream = new CryptoStream(container, decryptor, CryptoStreamMode.Write))
                {
                    await decryptedStream.WriteAsync(message, 0, message.Length);
                    decryptedStream.FlushFinalBlock();

                    byte[] array = container.ToArray();
                    var realLength = BitConverter.ToInt32(array.Take(4).ToArray(), 0);
                    decrypted = array.Skip(4).Take(realLength).ToArray();
                }
            }

            return decrypted;
        }

        public byte[] GenerateKey()
        {
            return this.GenerateBytes(this.KeySize / 8);
        }

        public byte[] GenerateIv()
        {
            return this.GenerateBytes(this.KeySize / 16);
        }

        private byte[] GenerateBytes(int length)
        {
            byte[] key = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);

                return key;
            }
        }
    }
}
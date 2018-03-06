namespace Hms.Common
{
    using System;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using Hms.Common.ElGamal;
    using Hms.Common.Interface;

    public class ElGamalCryptoProvider : IAsymmetricCryptoProvider
    {
        public int KeySize => 384;

        public Task<byte[]> EncryptBytesAsync(byte[] message, byte[] key)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            ElGamalKey elGamalKey;

            if (!ElGamalKey.TryParseBytes(key, out elGamalKey))
            {
                throw new ArgumentException($"{nameof(key)} has invalid format");       
            }

            using (var encryptor = new ElGamalEncryptor(elGamalKey))
            {
                return Task.FromResult(encryptor.ProcessData(message));
            }
        }

        public Task<byte[]> DecryptBytesAsync(byte[] message, byte[] key)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            ElGamalKey elGamalKey;

            if (!ElGamalKey.TryParseBytes(key, out elGamalKey))
            {
                throw new ArgumentException($"{nameof(key)} has invalid format");
            }

            var decryptor = new ElGamalDecryptor(elGamalKey);

            return Task.FromResult(decryptor.ProcessData(message));
        }

        public byte[] GeneratePrivateKey()
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var p = generator.GenPseudoPrime(this.KeySize, 16);
                var g = generator.GenRandomBits(this.KeySize - 1);
                var x = generator.GenRandomBits(this.KeySize - 1);
                var y = BigInteger.ModPow(g, x, p);

                var key = new ElGamalKey
                {
                    P = p,
                    G = g,
                    X = x,
                    Y = y
                };

                return key.ToBytes();
            }
        }

        public byte[] GetPublicKey(byte[] privateKey)
        {
            if (privateKey == null)
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            ElGamalKey key;
            if (ElGamalKey.TryParseBytes(privateKey, out key))
            {
                key.X = BigInteger.Zero;

                return key.ToBytes();
            }

            throw new ArgumentException("Invalid key");
        }
    }
}

namespace Hms.Common
{
    using System;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using Hms.Common.ElGamal;

    public class ElGamalCryptoProvider : IAsymmetricCryptoProvider
    {
        public int KeySize => 384;

        public Task<byte[]> EncryptAsync(byte[] message, byte[] key)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!ElGamalKey.TryParseBytes(key, out ElGamalKey elGamalKey))
            {
                throw new ArgumentException($"{nameof(key)} has invalid format");       
            }

            using (var encryptor = new ElGamalEncryptor(elGamalKey))
            {
                return Task.FromResult(encryptor.ProcessData(message));
            }
        }

        public Task<byte[]> DecryptAsync(byte[] message, byte[] key)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!ElGamalKey.TryParseBytes(key, out ElGamalKey elGamalKey))
            {
                throw new ArgumentException($"{nameof(key)} has invalid format");
            }

            var decryptor = new ElGamalDecryptor(elGamalKey);

            return Task.FromResult(decryptor.ProcessData(message));
        }

        public byte[] GenerateKey()
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
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElGamalEncryptor.cs" company="OOO 'OOO'">
//   2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hms.Common.ElGamal
{
    using System;
    using System.Linq;
    using System.Numerics;
    using System.Security.Cryptography;

    /// <summary>
    /// The el gamal encryptor.
    /// </summary>
    public class ElGamalEncryptor : ElGamalDataProcessor, IDisposable
    {
        /// <summary>
        /// The rng.
        /// </summary>
        private readonly RandomNumberGenerator rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElGamalEncryptor"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public ElGamalEncryptor(ElGamalKey key) : base(key)
        {
            this.rng = RandomNumberGenerator.Create();
        }
        
        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.rng.Dispose();
        }

        /// <summary>
        /// The process block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        protected override byte[] ProcessBlock(byte[] block)
        {
            BigInteger k;

            do
            {
                k = this.rng.GenRandomBits(this.Key.P.BitCount() - 1);
            }
            while (BigInteger.GreatestCommonDivisor(k, this.Key.P - 1) != 1);

            var a = BigInteger.ModPow(this.Key.G, k, this.Key.P);
            var b = (BigInteger.ModPow(this.Key.Y, k, this.Key.P) * new BigInteger(block.Concat(new byte[] { 0x00 }).ToArray())) % this.Key.P; // Make BigInteger unsigned

            var bytesA = a.ToByteArray();
            var bytesB = b.ToByteArray();

            var result = new byte[this.CiphertextBlockSize];

            Array.Copy(bytesA, 0, result, 0, bytesA.Length);
            Array.Copy(bytesB, 0, result, result.Length / 2, bytesB.Length);

            return result;
        }

        /// <summary>
        /// The process last block.
        /// </summary>
        /// <param name="lastBlock">
        /// The last block.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        protected override byte[] ProcessLastBlock(byte[] lastBlock)
        {
            return lastBlock.Length > 0 ? this.ProcessBlock(this.PadPlaintextBlock(lastBlock)) : new byte[0];
        }

        /// <summary>
        /// The pad plaintext block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        protected byte[] PadPlaintextBlock(byte[] block)
        {
            if (block.Length < this.BlockSize)
            {
                var paddedBlock = new byte[this.BlockSize];
                Array.Copy(block, 0, paddedBlock, 0, block.Length);
                return paddedBlock;
            }

            return block;
        }
    }
}
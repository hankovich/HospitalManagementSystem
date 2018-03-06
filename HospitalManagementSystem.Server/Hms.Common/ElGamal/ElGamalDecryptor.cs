// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElGamalDecryptor.cs" company="OOO 'OOO'">
//   2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hms.Common.ElGamal
{
    using System;
    using System.Linq;
    using System.Numerics;

    /// <summary>
    /// The el gamal decryptor.
    /// </summary>
    public class ElGamalDecryptor : ElGamalDataProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElGamalDecryptor"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public ElGamalDecryptor(ElGamalKey key)
            : base(key)
        {
            this.BlockSize = this.CiphertextBlockSize;
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
            var byteLength = this.CiphertextBlockSize / 2;
            var bytesA = new byte[byteLength];
            Array.Copy(block, 0, bytesA, 0, bytesA.Length);
            var bytesB = new byte[byteLength];
            Array.Copy(block, block.Length - bytesB.Length, bytesB, 0, bytesB.Length);

            var a = new BigInteger(bytesA);
            var b = new BigInteger(bytesB);

            a = BigInteger.ModPow(a, this.Key.X, this.Key.P);
            a = a.ModInverse(this.Key.P);
            var m = (b * a) % this.Key.P;

            var result = m.ToByteArray().Take(this.PlaintextBlockSize).ToArray();

            if (result.Length < this.PlaintextBlockSize)
            {
                var paddedBlock = new byte[this.PlaintextBlockSize];
                Array.Copy(result, 0, paddedBlock, 0, result.Length);
                result = paddedBlock;
            }

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
            return lastBlock.Length > 0 ? this.UnpadPlaintextBlock(this.ProcessBlock(lastBlock)) : new byte[0];
        }

        /// <summary>
        /// The unpad plaintext block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        protected byte[] UnpadPlaintextBlock(byte[] block)
        {
            var j = block.Length - 1;
            for (; j >= 0; j--)
            {
                if (block[j] != 0)
                {
                    break;
                }
            }

            var result = block.Take(j + 1).ToArray();
            return result;
        }
    }
}
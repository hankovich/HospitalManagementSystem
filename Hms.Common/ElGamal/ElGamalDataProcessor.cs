// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElGamalDataProcessor.cs" company="OOO 'OOO'">
//   2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hms.Common.ElGamal
{
    using System;
    using System.IO;

    /// <summary>
    /// The el gamal data processor.
    /// </summary>
    public abstract class ElGamalDataProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElGamalDataProcessor"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        protected ElGamalDataProcessor(ElGamalKey key)
        {
            this.Key = key;

            this.PlaintextBlockSize = key.GetPlaintextBlockSize();
            this.CiphertextBlockSize = key.GetCiphertextBlockSize();

            this.BlockSize = this.PlaintextBlockSize;
        }

        /// <summary>
        /// Gets or sets the block size.
        /// </summary>
        protected int BlockSize { get; set; }

        /// <summary>
        /// Gets or sets the plaintext block size.
        /// </summary>
        protected int PlaintextBlockSize { get; set; }

        /// <summary>
        /// Gets or sets the ciphertext block size.
        /// </summary>
        protected int CiphertextBlockSize { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        protected ElGamalKey Key { get; set; }

        /// <summary>
        /// The process data.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        public byte[] ProcessData(byte[] data)
        {
            var blocksCount = (data.Length / this.BlockSize) + (data.Length % this.BlockSize > 0 ? 1 : 0);
            blocksCount = Math.Max(blocksCount - 1, 0);

            if (blocksCount == 0)
            {
                return this.ProcessLastBlock(data);
            }

            using (var stream = new MemoryStream())
            {
                var block = new byte[this.BlockSize];

                var i = 0;
                for (; i < blocksCount; i++)
                {
                    Array.Copy(data, i * this.BlockSize, block, 0, this.BlockSize);

                    var result = this.ProcessBlock(block);

                    stream.Write(result, 0, result.Length);
                }

                var lastBlock = new byte[data.Length - (blocksCount * this.BlockSize)];
                Array.Copy(data, i * this.BlockSize, lastBlock, 0, lastBlock.Length);

                var processedLastBlock = this.ProcessLastBlock(lastBlock);

                stream.Write(processedLastBlock, 0, processedLastBlock.Length);

                return stream.ToArray();
            }
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
        protected abstract byte[] ProcessBlock(byte[] block);

        /// <summary>
        /// The process last block.
        /// </summary>
        /// <param name="lastBlock">
        /// The last block.
        /// </param>
        /// <returns>
        /// The <see cref="T:byte[]"/>.
        /// </returns>
        protected abstract byte[] ProcessLastBlock(byte[] lastBlock);
    }
}

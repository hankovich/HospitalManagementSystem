// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetKeyModel.cs" company="OOO 'OOO'">
//   2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hms.Common.Interface.Models
{
    /// <summary>
    /// The set key model.
    /// </summary>
    public class SetKeyModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>
        /// Gets or sets the round key.
        /// </summary>
        public byte[] RoundKey { get; set; }

        /// <summary>
        /// Gets or sets the initialization vector used to encrypt <see cref="ClientSecret"/>.
        /// </summary>
        public byte[] Iv { get; set; }
    }
}
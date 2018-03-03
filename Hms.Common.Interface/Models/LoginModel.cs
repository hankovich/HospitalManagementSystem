// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="OOO 'OOO'">
//   2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hms.Common.Interface.Models
{
    /// <summary>
    /// The login model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
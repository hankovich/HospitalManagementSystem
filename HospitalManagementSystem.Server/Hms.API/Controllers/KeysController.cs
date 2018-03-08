namespace Hms.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Common.Interface;
    using Hms.Common.Interface.Extensions;
    using Hms.Common.Interface.Models;
    using Hms.Services.Interface;

    public class KeysController : ApiController
    {
        public IAsymmetricCryptoProvider AsymmetricCryptoProvider { get; set; }

        public ISymmetricCryptoProvider SymmetricCryptoProvider { get; set; }

        public IGadgetKeysService GadgetKeysService { get; set; }

        public KeysController(
            IGadgetKeysService keysService,
            ISymmetricCryptoProvider symmetricCryptoProvider,
            IAsymmetricCryptoProvider asymmetricCryptoProvider)
        {
            if (keysService == null)
            {
                throw new ArgumentNullException(nameof(keysService));
            }

            if (symmetricCryptoProvider == null)
            {
                throw new ArgumentNullException(nameof(symmetricCryptoProvider));
            }

            if (asymmetricCryptoProvider == null)
            {
                throw new ArgumentNullException(nameof(asymmetricCryptoProvider));
            }

            this.GadgetKeysService = keysService;
            this.SymmetricCryptoProvider = symmetricCryptoProvider;
            this.AsymmetricCryptoProvider = asymmetricCryptoProvider;
        }

        [Route("api/key/public"), HttpPut]
        public async Task<IHttpActionResult> SetPublicKey([FromBody] SetKeyModel model)
        {
            if (model?.Identifier == null || model.Key == null)
            {
                return this.BadRequest("Invalid arguments");
            }

            try
            {
                if (string.IsNullOrEmpty(model.ClientSecret))
                {
                    var clientSecretBytes = new byte[255];
                    var rnd = new Random();
                    rnd.NextBytes(clientSecretBytes);
                    var clientSecret = Convert.ToBase64String(clientSecretBytes);
                    model.ClientSecret = clientSecret;
                }
                else
                {
                    var roundKey = await this.GadgetKeysService.GetGadgetRoundKeyAsync(model.Identifier);

                    model.ClientSecret = await this.SymmetricCryptoProvider.DecryptBase64StringAsync(model.ClientSecret, roundKey, model.Iv);
                }

                await this.GadgetKeysService.SetGadgetPublicKeyAsync(model.Identifier, model.ClientSecret, model.Key);

                var newRoundKey = this.SymmetricCryptoProvider.GenerateKey();
                model.RoundKey = newRoundKey;

                await this.GadgetKeysService.SetGadgetRoundKeyAsync(model.Identifier, model.ClientSecret, newRoundKey);

                var encryptedSecretBytes =
                    await
                    this.AsymmetricCryptoProvider.EncryptBytesAsync(
                        Convert.FromBase64String(model.ClientSecret),
                        model.Key);
                var encryptedSecret = Convert.ToBase64String(encryptedSecretBytes);

                var encryptedRoundKey = await this.AsymmetricCryptoProvider.EncryptBytesAsync(model.RoundKey, model.Key);

                model.ClientSecret = encryptedSecret;
                model.RoundKey = encryptedRoundKey;

                return this.Ok(model);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("api/key/round/{identifier}/"), HttpPut, Encrypted(Policy = ExpirationPolicy.AllowExpired)]
        public async Task<IHttpActionResult> GetNewRoundKey(
            string identifier,
            [FromBody] string clientSecret)
        {
            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(clientSecret))
            {
                return this.BadRequest("Invalid arguments");
            }

            try
            {
                string originalClientSecret = await this.GadgetKeysService.GetGadgetClientSecretAsync(identifier);

                if (clientSecret != originalClientSecret)
                {
                    return this.BadRequest("Invalid client secret");
                }

                var roundKey = this.SymmetricCryptoProvider.GenerateKey();
                await this.GadgetKeysService.SetGadgetRoundKeyAsync(identifier, originalClientSecret, roundKey);

                return this.Ok(roundKey);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
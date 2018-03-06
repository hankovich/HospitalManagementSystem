namespace Hms.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

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

                    model.ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64StringAsync(model.ClientSecret, roundKey, model.Iv);
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

        [Route("api/key/round/{identifier}/"), HttpPut]
        public async Task<IHttpActionResult> GetNewRoundKey(
            string identifier,
            [FromBody] EncryptedModel<string> encryptedClientSecret)
        {
            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(encryptedClientSecret?.Value))
            {
                return this.BadRequest("Invalid arguments");
            }

            try
            {
                byte[] oldRoundKey = await this.GadgetKeysService.GetGadgetRoundKeyAsync(identifier);

                var decryptedClientSecret =
                    await
                    this.SymmetricCryptoProvider.DecryptBase64StringAsync(
                        encryptedClientSecret.Value,
                        oldRoundKey,
                        encryptedClientSecret.Iv);

                string clientSecret = await this.GadgetKeysService.GetGadgetClientSecretAsync(identifier);

                if (decryptedClientSecret != clientSecret)
                {
                    return this.BadRequest("Invalid client secret");
                }

                var roundKey = this.SymmetricCryptoProvider.GenerateKey();
                await this.GadgetKeysService.SetGadgetRoundKeyAsync(identifier, clientSecret, roundKey);

                byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

                var encryptedNewRoundKey =
                    await this.SymmetricCryptoProvider.EncryptBytesAsync(roundKey, oldRoundKey, iv);

                return this.Ok(new EncryptedModel<byte[]> { Iv = iv, Value = encryptedNewRoundKey });
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
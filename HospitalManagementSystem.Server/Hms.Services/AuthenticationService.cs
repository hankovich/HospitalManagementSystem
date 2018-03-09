﻿namespace Hms.Services
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Extensions;
    using Hms.Common.Interface.Models;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Newtonsoft.Json;

    using Ninject;

    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService(ISymmetricCryptoProvider cryptoService, IGadgetKeysInfoRepository keysInfoRepository, IUserService userService)
        {
            if (cryptoService == null)
            {
                throw new ArgumentNullException(nameof(cryptoService));
            }

            if (keysInfoRepository == null)
            {
                throw new ArgumentNullException(nameof(keysInfoRepository));
            }

            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.SymmetricCryptoService = cryptoService;
            this.GadgetKeysInfoRepository = keysInfoRepository;
            this.UserService = userService;
        }

        public ISymmetricCryptoProvider SymmetricCryptoService { get; }

        public IGadgetKeysInfoRepository GadgetKeysInfoRepository { get; }

        public IUserService UserService { get; }

        [Inject]
        public RoundKeyExpirationSettings KeyExpirationSettings { get; set; }

        public async Task<AuthenticationResult> AuthenticateAsync(string authenticationToken)
        {
            try
            {
                if (authenticationToken == null)
                {
                    throw new ArgumentException("Auth header is not set");
                }

                string serializedModel = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                AuthHeaderModel model = JsonConvert.DeserializeObject<AuthHeaderModel>(serializedModel);

                string clientSecret = await this.GadgetKeysInfoRepository.GetGadgetClientSecretAsync(model.Indentifier);
                KeysInfoModel keys = await this.GadgetKeysInfoRepository.GetGadgetKeysInfoAsync(model.Indentifier, clientSecret);
                byte[] roundKey = keys.RoundKey;

                string login = await this.SymmetricCryptoService.DecryptBase64ToUtf8Async(model.Login, roundKey, model.Iv);
                string password = await this.SymmetricCryptoService.DecryptBase64ToUtf8Async(model.Password, roundKey, model.Iv);
                string decryptedClientSecret = await this.SymmetricCryptoService.DecryptBase64ToBase64Async(model.ClientSecret, roundKey, model.Iv);

                if (clientSecret != decryptedClientSecret)
                {
                    throw new ArgumentException("Invalid client secret");
                }

                bool isRoundKeyExpired = this.CheckIfRoundKeyExpired(keys);

                PrincipalModel principal = null;

                if (await this.UserService.CheckCredentials(login, password))
                {
                    principal = new PrincipalModel { Login = login };
                }

                return new AuthenticationResult
                {
                    IsAuthenticated = true,
                    IsRoundKeyExpired = isRoundKeyExpired,
                    RoundKey = roundKey,
                    Principal = principal
                };
            }
            catch (Exception e)
            {
                string failureReason = e is SqlException ? null : e.Message;

                return new AuthenticationResult
                {
                    FailureReason = failureReason,
                    IsAuthenticated = false
                };
            }
        }

        private bool CheckIfRoundKeyExpired(KeysInfoModel keys)
        {
            if (this.KeyExpirationSettings == null)
            {
                return false;
            }

            return keys.RoundKeySentTimes > this.KeyExpirationSettings.Requests || DateTime.UtcNow - keys.GeneratedTimeUtc > this.KeyExpirationSettings.Time;
        }
    }
}
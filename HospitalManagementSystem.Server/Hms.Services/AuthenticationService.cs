namespace Hms.Services
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Models;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    public class AuthenticationService : IAuthenticationService
    {
        public ISymmetricCryptoProvider SymmetricCryptoService { get; }

        public IGadgetKeysInfoRepository GadgetKeysInfoRepository { get; }

        public IUserRepository UserRepository { get; set; }

        public int RoundKeyIsValidForRequestCount { get; }

        public TimeSpan RoundKeyIsValidForTime { get; }

        public AuthenticationService(ISymmetricCryptoProvider cryptoService, IGadgetKeysInfoRepository keysInfoRepository, IUserRepository userRepository)
        {
            if (cryptoService == null)
            {
                throw new ArgumentNullException(nameof(cryptoService));
            }

            if (keysInfoRepository == null)
            {
                throw new ArgumentNullException(nameof(keysInfoRepository));
            }

            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            this.SymmetricCryptoService = cryptoService;
            this.GadgetKeysInfoRepository = keysInfoRepository;
            this.UserRepository = userRepository;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string authenticationToken)
        {
            try
            {
                if (authenticationToken == null) // TODO: Refactor this
                {
                    throw new ArgumentException("Auth header is not set");
                }

                byte[] bytes = Convert.FromBase64String(authenticationToken);
                string token = Encoding.UTF8.GetString(bytes);
                string[] tokens = token.Split(':');

                if (tokens.Length != 5)
                {
                    throw new ArgumentException("Invalid tokens count in auth header");    
                }

                string gadgetIdentifier = tokens[0];
                byte[] encryptedUsernameBytes = Convert.FromBase64String(tokens[1]);
                byte[] encryptedPasswordBytes = Convert.FromBase64String(tokens[2]);
                byte[] encryptedClientSecretBytes = Convert.FromBase64String(tokens[3]);
                byte[] iv = Convert.FromBase64String(tokens[4]);

                string clientSecret = await this.GadgetKeysInfoRepository.GetGadgetClientSecretAsync(gadgetIdentifier);
                KeysInfoModel keys = await this.GadgetKeysInfoRepository.GetGadgetKeysInfoAsync(gadgetIdentifier, clientSecret);
                byte[] roundKey = keys.RoundKey;

                byte[] clientSecretBytes = await this.SymmetricCryptoService.DecryptBytesAsync(encryptedClientSecretBytes, roundKey, iv);

                string decryptedClientSecret = Convert.ToBase64String(clientSecretBytes);

                if (clientSecret != decryptedClientSecret)
                {
                    throw new ArgumentException("Invalid client secret");
                }
                
                string username;

                bool isRoundKeyExpired = keys.RoundKeySentTimes > this.RoundKeyIsValidForRequestCount || DateTime.UtcNow - keys.GeneratedTimeUtc > this.RoundKeyIsValidForTime;

                try
                {
                    byte[] usernameBytes = await this.SymmetricCryptoService.DecryptBytesAsync(encryptedUsernameBytes, roundKey, iv);
                    byte[] passwordBytes = await this.SymmetricCryptoService.DecryptBytesAsync(encryptedPasswordBytes, roundKey, iv);

                    username = Encoding.UTF8.GetString(usernameBytes);
                    string password = Encoding.UTF8.GetString(passwordBytes);

                    await this.UserRepository.GetUserAsync(username, password);
                }
                catch
                {
                    return new AuthenticationResult
                    {
                        IsAuthenticated = true,
                        IsRoundKeyExpired = isRoundKeyExpired,
                        RoundKey = roundKey,
                    };
                }

                return new AuthenticationResult
                {
                    IsAuthenticated = true,
                    IsRoundKeyExpired = isRoundKeyExpired,
                    RoundKey = roundKey,
                    Principal = new PrincipalModel
                    {
                        Login = username,
                    }
                };
            }
            catch (Exception e) when (e is SqlException == false)
            {
                return new AuthenticationResult
                {
                    FailureReason = e.Message,
                    IsAuthenticated = false
                };
            }
            catch
            {
                return new AuthenticationResult
                {
                    IsAuthenticated = false
                };
            }
        }
    }
}

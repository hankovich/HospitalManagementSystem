namespace Hms.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Exceptions;
    using Hms.Common.Interface.Extensions;
    using Hms.Common.Interface.Models;
    using Hms.Services.Interface;

    using Newtonsoft.Json;

    public class Client : IClient
    {
        public ISymmetricCryptoProvider SymmetricCryptoProvider { get; }

        public IAsymmetricCryptoProvider AsymmetricCryptoProvider { get; }

        public IHttpContentService HttpContentService { get; }

        public Client(
            ISymmetricCryptoProvider symmetricCryptoProvider,
            IAsymmetricCryptoProvider asymmetricCryptoProvider,
            IHttpContentService httpContentService)
        {
            this.SymmetricCryptoProvider = symmetricCryptoProvider;
            this.AsymmetricCryptoProvider = asymmetricCryptoProvider;
            this.HttpContentService = httpContentService;
        }

        private async Task InitializeKeysAsync()
        {
            this.AuthInfo = new LoginModel { Identifier = Guid.NewGuid().ToString() };

            this.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.PrivateKey);

            SetKeyModel content = new SetKeyModel { Identifier = this.AuthInfo.Identifier, Key = publicKey };

            ServerResponse result = await this.SendNotEncryptedAsync(HttpMethod.Put, "api/key/public", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HmsException(result.ReasonPhrase);
            }

            SetKeyModel setKeyModel = JsonConvert.DeserializeObject<SetKeyModel>(result.Content);
            byte[] enryptedClientSecretBytes = Convert.FromBase64String(setKeyModel.ClientSecret);
            string clientSecret =
                Convert.ToBase64String(
                    await this.AsymmetricCryptoProvider.DecryptBytesAsync(enryptedClientSecretBytes, this.PrivateKey));

            this.RoundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(setKeyModel.RoundKey, this.PrivateKey);

            this.AuthInfo.ClientSecret = clientSecret;

            this.IsInitialized = true;
        }

        public string Host => "http://localhost:52017/";

        private bool IsInitialized { get; set; }

        private LoginModel AuthInfo { get; set; }

        private byte[] PrivateKey { get; set; }

        private byte[] RoundKey { get; set; }

        private HttpClient HttpClient { get; } = new HttpClient();

        public async Task<ServerResponse> SendAsync(HttpMethod method, string url, object content)
        {
            if (!this.IsInitialized)
            {
                await this.InitializeKeysAsync();
            }

            using (var request = new HttpRequestMessage(method, this.Host + url))
            {
                request.Content = content == null
                                      ? null
                                      : await
                                        this.HttpContentService.EncryptAsync(GetHttpContent(content), this.RoundKey);
                request.Headers.Authorization = await this.CreateCredentialsAsync();

                HttpResponseMessage response = await this.HttpClient.SendAsync(request);

                string responseContent = await response.Content.ReadAsStringAsync();
                var result = new ServerResponse
                {
                    Content = responseContent,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };

                //// TODO: Handle 205 Status Code

                return result;
            }
        }

        public async Task<ServerResponse> SendNotEncryptedAsync(HttpMethod method, string url, object content)
        {
            using (var request = new HttpRequestMessage(method, this.Host + url))
            {
                request.Content = content == null ? null : GetHttpContent(content);
                request.Headers.Authorization = await this.CreateCredentialsAsync();

                HttpResponseMessage response = await this.HttpClient.SendAsync(request);

                string responseContent = await response.Content.ReadAsStringAsync();
                var result = new ServerResponse
                {
                    Content = responseContent,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    StatusCode = (int)response.StatusCode
                };

                return result;
            }
        }

        public async Task LoginAsync(string username, string password)
        {
            if (!this.IsInitialized)
            {
                await this.InitializeKeysAsync();
            }
        }

        public async Task RegisterAsync(string username, string password)
        {
            if (!this.IsInitialized)
            {
                await this.InitializeKeysAsync();
            }

            byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

            LoginModel model = new LoginModel
            {
                Identifier = this.AuthInfo.Identifier,
                ClientSecret =
                    await
                    this.SymmetricCryptoProvider.EncryptBase64StringAsync(this.AuthInfo.ClientSecret, this.RoundKey, iv),
                Username = await this.SymmetricCryptoProvider.EncryptUtf8StringAsync(username, this.RoundKey, iv),
                Password = await this.SymmetricCryptoProvider.EncryptUtf8StringAsync(password, this.RoundKey, iv)
            };

            ServerResponse response = await this.SendAsync(HttpMethod.Post, "api/account", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            this.AuthInfo.Username = username;
            this.AuthInfo.Password = password;
        }

        public Task LogoutAsync()
        {
            this.AuthInfo.Username = this.AuthInfo.Password = null;
            return Task.CompletedTask;
        }

        private async Task<AuthenticationHeaderValue> CreateCredentialsAsync()
        {
            byte[] ivBytes = this.SymmetricCryptoProvider.GenerateIv();

            string username =
                await
                this.SymmetricCryptoProvider.EncryptUtf8StringAsync(this.AuthInfo.Username, this.RoundKey, ivBytes);
            string password =
                await
                this.SymmetricCryptoProvider.EncryptUtf8StringAsync(this.AuthInfo.Password, this.RoundKey, ivBytes);
            string clientSecret =
                await
                this.SymmetricCryptoProvider.EncryptBase64StringAsync(this.AuthInfo.ClientSecret, this.RoundKey, ivBytes);
            string iv = Convert.ToBase64String(ivBytes);

            string s = $"{this.AuthInfo.Identifier}:{username}:{password}:{clientSecret}:{iv}";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string parameter = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", parameter);
        }

        private static HttpContent GetHttpContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
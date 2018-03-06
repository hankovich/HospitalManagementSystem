namespace Hms.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Exceptions;
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

            Task.Run(async () => await this.InitializeAsync());
        }

        private async Task InitializeAsync()
        {
            this.AuthInfo = new LoginModel { Identifier = Guid.NewGuid().ToString() };

            this.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.PrivateKey);

            SetKeyModel content = new SetKeyModel { Identifier = this.AuthInfo.Identifier, Key = publicKey };

            ServerResponse result = await this.SendAsync(HttpMethod.Put, "api/key/public", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HmsException(result.ReasonPhrase);
            }

            SetKeyModel setKeyModel = JsonConvert.DeserializeObject<SetKeyModel>(result.Content);
            byte[] enryptedClientSecretBytes = Convert.FromBase64String(setKeyModel.ClientSecret);
            string clientSecret =
                Convert.ToBase64String(await this.AsymmetricCryptoProvider.DecryptBytesAsync(enryptedClientSecretBytes, this.PrivateKey));

            this.RoundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(setKeyModel.RoundKey, this.PrivateKey);

            this.AuthInfo.ClientSecret = clientSecret;
        }

        public string Host => "http://localhost:52017/";

        private LoginModel AuthInfo { get; set; }

        private byte[] PrivateKey { get; set; }

        private byte[] RoundKey { get; set; }

        private HttpClient HttpClient { get; } = new HttpClient();

        public async Task<ServerResponse> SendAsync(HttpMethod method, string url, object content)
        {
            using (var request = new HttpRequestMessage(method, this.Host + url))
            {
                request.Content = content == null ? null : await this.HttpContentService.EncryptAsync(GetHttpContent(content), this.RoundKey);
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

        public async Task LoginAsync(string username, string password)
        {
            
        }

        public async Task RegisterAsync(string username, string password)
        {
            
        }

        public Task LogoutAsync()
        {
            this.AuthInfo.Username = this.AuthInfo.Password = null;
            return Task.CompletedTask;
        }

        private async Task<AuthenticationHeaderValue> CreateCredentialsAsync()
        {
            if (this.AuthInfo == null)
            {
                return null;
            }

            byte[] ivBytes = this.SymmetricCryptoProvider.GenerateIv();

            byte[] encryptedUsernameBytes = await this.SymmetricCryptoProvider.EncryptBytesAsync(
                                                Encoding.UTF8.GetBytes(this.AuthInfo.Username),
                                                this.RoundKey,
                                                ivBytes);

            byte[] encryptedPasswordBytes = await this.SymmetricCryptoProvider.EncryptBytesAsync(
                                                Encoding.UTF8.GetBytes(this.AuthInfo.Password),
                                                this.RoundKey,
                                                ivBytes);

            byte[] encryptedClientSecretBytes = await this.SymmetricCryptoProvider.EncryptBytesAsync(
                                                    Convert.FromBase64String(this.AuthInfo.ClientSecret),
                                                    this.RoundKey,
                                                    ivBytes);

            string username = Convert.ToBase64String(encryptedUsernameBytes);
            string password = Convert.ToBase64String(encryptedPasswordBytes);
            string clientSecret = Convert.ToBase64String(encryptedClientSecretBytes);
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
namespace Hms.Services
{
    using System;
    using System.Net;
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

        public string Host => "http://localhost:52017/";

        private bool IsInitialized { get; set; }

        private LoginModel AuthInfo { get; set; }

        private GadgetInfoModel GadgetInfo { get; set; }

        private byte[] PrivateKey { get; set; }

        private byte[] RoundKey { get; set; }

        private HttpClient HttpClient { get; } = new HttpClient();

        private async Task InitializeKeysAsync()
        {
            this.AuthInfo = new LoginModel(); // TODO: Critical section 
            this.GadgetInfo = new GadgetInfoModel { Identifier = Guid.NewGuid().ToString() };

            this.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.PrivateKey);

            SetKeyModel content = new SetKeyModel { Identifier = this.GadgetInfo.Identifier, Key = publicKey };

            ServerResponse<SetKeyModel> result = await this.SendAsync<SetKeyModel>(HttpMethod.Put, "api/key/public", content, false);

            if (!result.IsSuccessStatusCode)
            {
                throw new HmsException(result.ReasonPhrase);
            }

            SetKeyModel setKeyModel = result.Content;
            byte[] enryptedClientSecretBytes = Convert.FromBase64String(setKeyModel.ClientSecret);
            string clientSecret =
                Convert.ToBase64String(
                    await this.AsymmetricCryptoProvider.DecryptBytesAsync(enryptedClientSecretBytes, this.PrivateKey));

            this.RoundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(setKeyModel.RoundKey, this.PrivateKey);

            this.GadgetInfo.ClientSecret = clientSecret;

            this.IsInitialized = true;
        }

        public async Task ChangeRoundKey()
        {
            ServerResponse<string> response =
                await SendAsync<string>(HttpMethod.Put, $"api/key/round/{this.GadgetInfo.Identifier}/", this.GadgetInfo.ClientSecret);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            this.RoundKey = Convert.FromBase64String(response.Content);
        }

        public async Task ChangeAsymmetricKey()
        {
            this.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.PrivateKey);
            byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

            SetKeyModel content = new SetKeyModel
            {
                Identifier = this.GadgetInfo.Identifier,
                Key = publicKey,
                ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64ToBase64Async(this.GadgetInfo.ClientSecret, this.RoundKey, iv),
                Iv = iv
            };

            ServerResponse<SetKeyModel> response = await this.SendAsync<SetKeyModel>(HttpMethod.Put, "api/key/public", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            SetKeyModel encryptedLoginModel = response.Content;
            byte[] encryptedRoundKey = encryptedLoginModel.RoundKey;
            byte[] roundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(encryptedRoundKey, this.PrivateKey);
            this.RoundKey = roundKey;
        }

        public async Task<ServerResponse<TContent>> SendAsync<TContent>(HttpMethod method, string url, object content, bool needsEncryption = true)
        {
            if (!this.IsInitialized && needsEncryption)
            {
                await this.InitializeKeysAsync();
            }

            using (var request = new HttpRequestMessage(method, this.Host + url))
            {
                request.Content = await this.SerializeToHttpContentAsync(content, needsEncryption);

                request.Headers.Authorization = await this.CreateCredentialsAsync();

                using (HttpResponseMessage response = await this.HttpClient.SendAsync(request))
                {
                    string responseString = await this.DeserializeFromHttpContentAsync(response.Content, needsEncryption && response.IsSuccessStatusCode);

                    TContent receivedContent = string.IsNullOrEmpty(responseString) ? default(TContent) : JsonConvert.DeserializeObject<TContent>(responseString);

                    var result = new ServerResponse<TContent>
                    {
                        Content = receivedContent,
                        IsSuccessStatusCode = response.IsSuccessStatusCode,
                        ReasonPhrase = response.ReasonPhrase,
                        StatusCode = (int)response.StatusCode
                    };

                    if (response.StatusCode == (HttpStatusCode)424)
                    {
                        await this.ChangeRoundKey();
                        result = await SendAsync<TContent>(method, url, content, needsEncryption);
                    }

                    return result;
                }
            }
        }

        public async Task LoginAsync(string login, string password)
        {
            if (!this.IsInitialized)
            {
                await this.InitializeKeysAsync();
            }

            LoginModel model = new LoginModel
            {
                Login = login,
                Password = password
            };

            ServerResponse<string> response = await this.SendAsync<string>(HttpMethod.Put, "api/account", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            this.AuthInfo.Login = login;
            this.AuthInfo.Password = password;
        }

        public async Task RegisterAsync(string login, string password)
        {
            if (!this.IsInitialized)
            {
                await this.InitializeKeysAsync();
            }

            LoginModel model = new LoginModel
            {
                Login = login,
                Password = password
            };

            ServerResponse<string> response = await this.SendAsync<string>(HttpMethod.Post, "api/account", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }
        }

        public Task LogoutAsync()
        {
            this.AuthInfo.Login = this.AuthInfo.Password = null;
            return Task.CompletedTask;
        }

        private async Task<AuthenticationHeaderValue> CreateCredentialsAsync()
        {
            byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

            AuthHeaderModel model = new AuthHeaderModel
            {
                Indentifier = this.GadgetInfo.Identifier,
                ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64ToBase64Async(this.GadgetInfo.ClientSecret, this.RoundKey, iv),
                Login = await this.SymmetricCryptoProvider.EncryptUtf8ToBase64Async(this.AuthInfo?.Login, this.RoundKey, iv),
                Password = await this.SymmetricCryptoProvider.EncryptUtf8ToBase64Async(this.AuthInfo?.Password, this.RoundKey, iv),
                Iv = iv
            };

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            return new AuthenticationHeaderValue("Hospital", Convert.ToBase64String(bytes));
        }

        private async Task<HttpContent> SerializeToHttpContentAsync(object content, bool needsEncryption)
        {
            HttpContent httpContent = ConvertToHttpContent(content);

            if (needsEncryption && httpContent != null)
            {
                return await this.HttpContentService.EncryptAsync(httpContent, this.RoundKey);
            }

            return httpContent;
        }

        private async Task<string> DeserializeFromHttpContentAsync(HttpContent content, bool needsDecryption)
        {
            if (content == null || (content.Headers.ContentLength ?? 0) == 0)
            {
                return null;
            }

            if (needsDecryption)
            {
                content = await this.HttpContentService.DecryptAsync(content, this.RoundKey);
            }

            return await content.ReadAsStringAsync();
        }

        private static HttpContent ConvertToHttpContent(object content)
        {
            if (content == null)
            {
                return null;
            }

            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
    }
}
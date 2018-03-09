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
            this.AuthInfo = new LoginModel();
            this.GadgetInfo = new GadgetInfoModel { Identifier = Guid.NewGuid().ToString() };

            this.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.PrivateKey);

            SetKeyModel content = new SetKeyModel { Identifier = this.GadgetInfo.Identifier, Key = publicKey };

            ServerResponse result = await this.SendAsync(HttpMethod.Put, "api/key/public", content, false);

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

            this.GadgetInfo.ClientSecret = clientSecret;

            this.IsInitialized = true;
        }

        public async Task ChangeRoundKey()
        {
            ServerResponse response =
                await SendAsync(HttpMethod.Put, $"api/key/round/{this.GadgetInfo.Identifier}/", this.GadgetInfo.ClientSecret);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            this.RoundKey = Convert.FromBase64String(JsonConvert.DeserializeObject<string>(response.Content));
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
                ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64StringAsync(this.GadgetInfo.ClientSecret, this.RoundKey, iv),
                Iv = iv
            };

            ServerResponse response = await this.SendAsync(HttpMethod.Put, "api/key/public", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            SetKeyModel encryptedLoginModel = JsonConvert.DeserializeObject<SetKeyModel>(response.Content);
            byte[] encryptedRoundKey = encryptedLoginModel.RoundKey;
            byte[] roundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(encryptedRoundKey, this.PrivateKey);
            this.RoundKey = roundKey;
        }

        public async Task<ServerResponse> SendAsync(HttpMethod method, string url, object content, bool needsEncryption = true)
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
                    string responseString = await this.DeserializeFromHttpContentAsync(response.Content, needsEncryption);

                    var result = new ServerResponse
                    {
                        Content = responseString,
                        IsSuccessStatusCode = response.IsSuccessStatusCode,
                        ReasonPhrase = response.ReasonPhrase,
                        StatusCode = (int)response.StatusCode
                    };

                    if (response.StatusCode == HttpStatusCode.ResetContent)
                    {
                        await this.ChangeRoundKey();
                        result = await SendAsync(method, url, content, needsEncryption);
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

            ServerResponse response = await this.SendAsync(HttpMethod.Put, "api/account", model);

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

            ServerResponse response = await this.SendAsync(HttpMethod.Post, "api/account", model);

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
            byte[] ivBytes = this.SymmetricCryptoProvider.GenerateIv();

            string username =
                await
                this.SymmetricCryptoProvider.EncryptUtf8StringAsync(this.AuthInfo?.Login, this.RoundKey, ivBytes);
            string password =
                await
                this.SymmetricCryptoProvider.EncryptUtf8StringAsync(this.AuthInfo?.Password, this.RoundKey, ivBytes);
            string clientSecret =
                await
                this.SymmetricCryptoProvider.EncryptBase64StringAsync(this.GadgetInfo.ClientSecret, this.RoundKey, ivBytes);
            string iv = Convert.ToBase64String(ivBytes);

            string s = $"{this.GadgetInfo.Identifier}:{username}:{password}:{clientSecret}:{iv}";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string parameter = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", parameter);
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
            string responseString = null;

            if (content != null && content.Headers.ContentLength != 0)
            {
                if (needsDecryption)
                {
                    content = await this.HttpContentService.DecryptAsync(content, this.RoundKey);
                }

                responseString = await content.ReadAsStringAsync();
            }

            return responseString;
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
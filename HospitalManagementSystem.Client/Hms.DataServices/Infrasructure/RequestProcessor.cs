namespace Hms.DataServices.Infrasructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Extensions;
    using Hms.Common.Interface.Models;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    using Newtonsoft.Json;

    public class RequestProcessor
    {
        public RequestProcessor(ISymmetricCryptoProvider symmetricCryptoProvider, IHttpContentProcessor httpContentService, ClientStateModel clientState)
        {
            this.SymmetricCryptoProvider = symmetricCryptoProvider;
            this.HttpContentProcessor = httpContentService;
            this.ClientState = clientState;
        }

        public ISymmetricCryptoProvider SymmetricCryptoProvider { get; }

        public IHttpContentProcessor HttpContentProcessor { get; }

        public ClientStateModel ClientState { get; }

        public bool NeedEncryption { get; set; }

        public async Task<ServerResponse<TContent>> ProcessResponseAsync<TContent>(HttpResponseMessage response)
        {
            string responseString = await this.HttpContentProcessor.DeserializeAsync(response.Content, this.ClientState.RoundKey, this.NeedEncryption && response.IsSuccessStatusCode);

            TContent receivedContent = string.IsNullOrEmpty(responseString) || !response.IsSuccessStatusCode
                                           ? default(TContent)
                                           : JsonConvert.DeserializeObject<TContent>(responseString);

            string reasonPhrase;

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    reasonPhrase = JsonConvert.DeserializeObject<dynamic>(responseString).Message;
                }
                catch
                {
                    reasonPhrase = response.ReasonPhrase;
                }
            }
            else
            {
                reasonPhrase = response.ReasonPhrase;
            }
            
            var result = new ServerResponse<TContent>
            {
                Content = receivedContent,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                ReasonPhrase = reasonPhrase,
                StatusCode = (int)response.StatusCode
            };

            return result;
        }

        public async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string requestUri, object content = null)
        {
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = await this.HttpContentProcessor.SerializeAsync(content, this.ClientState.RoundKey, this.NeedEncryption)
            };

            request.Headers.Authorization = await this.CreateCredentialsAsync();

            return request;
        }

        private async Task<AuthenticationHeaderValue> CreateCredentialsAsync()
        {
            byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

            AuthHeaderModel model = new AuthHeaderModel
            {
                Indentifier = this.ClientState.Identifier,
                ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64ToBase64Async(this.ClientState.ClientSecret, this.ClientState.RoundKey, iv),
                Login = await this.SymmetricCryptoProvider.EncryptUtf8ToBase64Async(this.ClientState.AuthInfo?.Login, this.ClientState.RoundKey, iv),
                Password = await this.SymmetricCryptoProvider.EncryptUtf8ToBase64Async(this.ClientState.AuthInfo?.Password, this.ClientState.RoundKey, iv),
                Iv = iv
            };

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            return new AuthenticationHeaderValue("Hospital", Convert.ToBase64String(bytes));
        }
    }
}
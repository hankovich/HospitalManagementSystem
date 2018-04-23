namespace Hms.DataServices.Infrasructure
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Hms.Common.Interface;
    using Hms.Common.Interface.Exceptions;
    using Hms.Common.Interface.Extensions;
    using Hms.Common.Interface.Models;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    using Nito.AsyncEx;

    public class RequestCoordinator : IRequestCoordinator
    {
        public ISymmetricCryptoProvider SymmetricCryptoProvider { get; }

        public IAsymmetricCryptoProvider AsymmetricCryptoProvider { get; }

        public IRequestProcessorBuilder RequestProcessorBuilder { get; }

        public RequestCoordinator(
            ISymmetricCryptoProvider symmetricCryptoProvider,
            IAsymmetricCryptoProvider asymmetricCryptoProvider,
            IRequestProcessorBuilder requestProcessorBuilder)
        {
            this.SymmetricCryptoProvider = symmetricCryptoProvider;
            this.AsymmetricCryptoProvider = asymmetricCryptoProvider;
            this.RequestProcessorBuilder = requestProcessorBuilder;

            this.ClientState = new ClientStateModel();
        }

        public string Host => /*"http://localhost:52017/"; //*/ "http://localhost.fiddler:52017/";

        public int? UserId { get; private set; }

        private bool IsInitialized { get; set; }

        private ClientStateModel ClientState { get; }

        private HttpClient HttpClient { get; } = new HttpClient();

        private static readonly SemaphoreSlim InitializationSemaphore = new SemaphoreSlim(1, 1);

        private static readonly SemaphoreSlim ChangeKeySemaphore = new SemaphoreSlim(1, 1);

        private static AsyncCountdownEvent CountdownEvent = new AsyncCountdownEvent(1);
        
        public async Task ChangeRoundKey()
        {
            await ChangeKeySemaphore.WaitAsync();
            CountdownEvent.Signal();
            await CountdownEvent.WaitAsync();
            CountdownEvent = new AsyncCountdownEvent(1);

            try
            {
                ServerResponse<string> response = await this.SendAsyncInternal<string>(HttpMethod.Put, $"api/key/round/{this.ClientState.Identifier}/", this.ClientState.ClientSecret, needsThreadSafe: false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HmsException(response.ReasonPhrase);
                }

                this.ClientState.RoundKey = Convert.FromBase64String(response.Content);
            }
            finally
            {
                ChangeKeySemaphore.Release();
            }
        }

        public async Task ChangeAsymmetricKey() // Not thread-safe
        {
            this.ClientState.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
            byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.ClientState.PrivateKey);
            byte[] iv = this.SymmetricCryptoProvider.GenerateIv();

            SetKeyModel content = new SetKeyModel
            {
                Identifier = this.ClientState.Identifier,
                Key = publicKey,
                ClientSecret = await this.SymmetricCryptoProvider.EncryptBase64ToBase64Async(this.ClientState.ClientSecret, this.ClientState.RoundKey, iv),
                Iv = iv
            };

            ServerResponse<SetKeyModel> response = await this.SendAsync<SetKeyModel>(HttpMethod.Put, "api/key/public", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            SetKeyModel encryptedLoginModel = response.Content;
            byte[] encryptedRoundKey = encryptedLoginModel.RoundKey;
            byte[] roundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(encryptedRoundKey, this.ClientState.PrivateKey);
            this.ClientState.RoundKey = roundKey;
        }

        public async Task<ServerResponse<TContent>> SendAsync<TContent>(
            HttpMethod method,
            string url,
            object content = null,
            bool needsEncryption = true)
        {
            return await this.SendAsyncInternal<TContent>(method, url, content, needsEncryption);
        }

        public async Task LoginAsync(string login, string password)
        {
            await this.EnsureKeysInitializationAsync();

            LoginModel model = new LoginModel { Login = login, Password = password };

            ServerResponse<int> response = await this.SendAsync<int>(HttpMethod.Put, "api/account", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            this.ClientState.AuthInfo.Login = login;
            this.ClientState.AuthInfo.Password = password;
            this.UserId = response.Content;
        }

        public async Task RegisterAsync(string login, string password)
        {
            await this.EnsureKeysInitializationAsync();

            LoginModel model = new LoginModel { Login = login, Password = password };

            ServerResponse<string> response = await this.SendAsync<string>(HttpMethod.Post, "api/account", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }
        }

        public Task LogoutAsync()
        {
            this.ClientState.AuthInfo.Login = this.ClientState.AuthInfo.Password = null;
            this.UserId = null;

            return Task.CompletedTask;
        }

        private async Task EnsureKeysInitializationAsync()
        {
            if (!this.IsInitialized)
            {
                await InitializationSemaphore.WaitAsync();

                if (!this.IsInitialized)
                {
                    try
                    {
                        this.ClientState.Identifier = Guid.NewGuid().ToString();

                        this.ClientState.PrivateKey = this.AsymmetricCryptoProvider.GeneratePrivateKey();
                        byte[] publicKey = this.AsymmetricCryptoProvider.GetPublicKey(this.ClientState.PrivateKey);

                        SetKeyModel content = new SetKeyModel { Identifier = this.ClientState.Identifier, Key = publicKey };

                        ServerResponse<SetKeyModel> result = await this.SendAsync<SetKeyModel>(HttpMethod.Put, "api/key/public", content, false);

                        if (!result.IsSuccessStatusCode)
                        {
                            throw new HmsException(result.ReasonPhrase);
                        }

                        SetKeyModel setKeyModel = result.Content;
                        string clientSecret = await this.AsymmetricCryptoProvider.DecryptBase64ToBase64Async(setKeyModel.ClientSecret, this.ClientState.PrivateKey);
                        this.ClientState.RoundKey = await this.AsymmetricCryptoProvider.DecryptBytesAsync(setKeyModel.RoundKey, this.ClientState.PrivateKey);

                        this.ClientState.ClientSecret = clientSecret;

                        this.IsInitialized = true;
                    }
                    finally
                    {
                        InitializationSemaphore.Release();
                    }
                }
            }
        }

        private async Task<ServerResponse<TContent>> SendAsyncInternal<TContent>(
            HttpMethod method,
            string url,
            object content = null,
            bool needsEncryption = true,
            bool needsThreadSafe = true)
        {
            if (needsEncryption)
            {
                await this.EnsureKeysInitializationAsync();
            }

            if (needsThreadSafe)
            {
                await AsyncFactory.FromWaitHandle(ChangeKeySemaphore.AvailableWaitHandle);
            }

            CountdownEvent.AddCount();

            RequestProcessor requestProcessor =
                this.RequestProcessorBuilder.UseEncryption(needsEncryption).Build(this.ClientState);

            using (var request = await requestProcessor.CreateRequestAsync(method, this.Host + url, content))
            {
                using (HttpResponseMessage response = await this.HttpClient.SendAsync(request).ContinueWith(this.DecrementEvent))
                {
                    if (response.StatusCode == (HttpStatusCode)424)
                    {
                        await this.ChangeRoundKey();
                        return await this.SendAsyncInternal<TContent>(method, url, content, needsEncryption, needsThreadSafe);
                    }

                    var result = await requestProcessor.ProcessResponseAsync<TContent>(response);

                    return result;
                }
            }
        }

        private HttpResponseMessage DecrementEvent(Task<HttpResponseMessage> task)
        {
            CountdownEvent.Signal();
            return task.Result;
        }
    }
}
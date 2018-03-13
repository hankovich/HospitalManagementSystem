namespace Hms.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    public class Service : IService
    {
        public Service(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; }

        private bool IsInitialized { get; set; }

        public async Task Do()
        {
            if (!this.IsInitialized)
            {
                await this.InitializeAsync();
            }

            try
            {
                var response = await this.Client.SendAsync<MedicalCard>(HttpMethod.Get, "api/card/0/10000", null);
            }
            catch (Exception e)
            {
                
            }

            await this.Client.ChangeRoundKey();
        }

        private async Task InitializeAsync()
        {
            this.IsInitialized = true;
        }
    }
}

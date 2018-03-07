namespace Hms.Services
{
    using System.Net.Http;

    using Hms.Services.Interface;

    public class Service : IService
    {
        public IClient Client { get; }

        public Service(IClient client)
        {
            this.Client = client;
        }

        public async void Do()
        {
            // await this.Client.RegisterAsync("1koala", "zakon");
            await this.Client.LoginAsync("1koala", "zakon");

            var a = await this.Client.SendAsync(HttpMethod.Get, "api/hello/get", null);
            await this.Client.ChangeRoundKey();
        }
    }
}

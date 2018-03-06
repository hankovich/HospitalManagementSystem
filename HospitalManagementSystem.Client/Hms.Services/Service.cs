namespace Hms.Services
{
    using Hms.Services.Interface;

    public class Service : IService
    {
        public IClient Client { get; }

        public Service(IClient client)
        {
            this.Client = client;
        }

        public void Do()
        {
        }
    }
}

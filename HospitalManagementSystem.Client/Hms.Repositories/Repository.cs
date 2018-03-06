namespace Hms.Repositories
{
    using Hms.Repositories.Interface;

    public class Repository : IRepository
    {
        public IClient Client { get; }

        public Repository(IClient client)
        {
            this.Client = client;
        }

        public void Do()
        {
        }
    }
}

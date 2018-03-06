namespace Hms.Services
{
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class Service : IService
    {
        public IRepository Repository { get; }

        public Service(IRepository repository)
        {
            this.Repository = repository;
        }

        public void Do()
        {
            this.Repository.Do();
        }
    }
}

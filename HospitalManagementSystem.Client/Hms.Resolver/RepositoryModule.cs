namespace Hms.Resolver
{
    using Hms.Repositories;
    using Hms.Repositories.Interface;

    using Ninject.Modules;

    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRepository>().To<Repository>();
        }
    }
}
namespace Hms.Resolver
{
    using Hms.Services;
    using Hms.Services.Interface;

    using Ninject.Modules;

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IService>().To<Service>();
        }
    }
}
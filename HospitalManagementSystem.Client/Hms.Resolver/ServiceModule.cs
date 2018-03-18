namespace Hms.Resolver
{
    using Hms.Services;
    using Hms.Services.Interface;

    using Ninject.Modules;

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IClient>().To<Client>().InSingletonScope();
            this.Bind<IMedicalCardService>().To<MedicalCardService>();
            this.Bind<IProfileService>().To<ProfileService>();
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IBuildingService>().To<BuildingService>();
            this.Bind<IPolyclinicRegionService>().To<PolyclinicRegionService>();
        }
    }
}
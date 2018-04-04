namespace Hms.Resolver
{
    using Hms.DataServices;
    using Hms.DataServices.Interface;

    using Ninject.Modules;

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRequestCoordinator>().To<RequestCoordinator>().InSingletonScope();
            this.Bind<IMedicalCardDataService>().To<MedicalCardDataService>();
            this.Bind<IProfileDataService>().To<ProfileDataService>();
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IBuildingDataService>().To<BuildingDataService>();
            this.Bind<IPolyclinicRegionDataService>().To<PolyclinicRegionDataService>();
        }
    }
}
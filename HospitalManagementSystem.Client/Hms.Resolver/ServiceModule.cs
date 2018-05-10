namespace Hms.Resolver
{
    using Hms.DataServices;
    using Hms.DataServices.Infrasructure;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    using Ninject.Modules;

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IRequestProcessorBuilder>().To<RequestProcessorBuilder>();
            this.Bind<IHttpContentProcessor>().To<HttpContentProcessor>();

            this.Bind<IAccountService>().To<AccountService>();

            this.Bind<IRequestCoordinator>().To<RequestCoordinator>().InSingletonScope();
            this.Bind<IMedicalCardDataService>().To<MedicalCardDataService>();
            this.Bind<IMedicalRecordDataService>().To<MedicalRecordDataService>();
            this.Bind<IProfileDataService>().To<ProfileDataService>();
            this.Bind<IBuildingDataService>().To<BuildingDataService>();
            this.Bind<IPolyclinicRegionDataService>().To<PolyclinicRegionDataService>();
            this.Bind<IAttachmentDataService>().To<AttachmentDataService>();
            this.Bind<IDoctorDataService>().To<DoctorDataService>();
            this.Bind<IMedicalSpecializationDataService>().To<MedicalSpecializationDataService>();
            this.Bind<IPolyclinicDataService>().To<PolyclinicDataService>();
        }
    }
}
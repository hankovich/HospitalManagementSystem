namespace Hms.Resolver
{
    using System;
    using System.Configuration;

    using Hms.Services;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Ninject.Modules;

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            string requestsString = ConfigurationManager.AppSettings["RoundKeyIsValidForRequests"];
            string hoursString = ConfigurationManager.AppSettings["RoundKeyIsValidForHours"];

            int requests;
            double hours;

            if (!int.TryParse(requestsString, out requests))
            {
                requests = int.MaxValue;
            }

            if (!double.TryParse(hoursString, out hours))
            {
                hours = double.MaxValue;
            }

            this.Bind<IUserService>().To<UserService>();
            this.Bind<IGadgetKeysService>().To<GadgetKeysService>();
            this.Bind<IMedicalCardService>().To<MedicalCardService>();
            this.Bind<IProfileService>().To<ProfileService>();
            this.Bind<IBuildingService>().To<BuildingService>();
            this.Bind<IPolyclinicRegionService>().To<PolyclinicRegionService>();
            this.Bind<IAttachmentService>().To<AttachmentService>();
            this.Bind<IDoctorService>().To<DoctorService>();
            this.Bind<IMedicalSpecializationService>().To<MedicalSpecializationService>();
            this.Bind<IPolyclinicService>().To<PolyclinicService>();

            this.Bind<IPolyclinicRegionProvider>().To<DummyPolyclinicRegionProvider>();
            
            this.Bind<RoundKeyExpirationSettings>().ToConstant(new RoundKeyExpirationSettings
            {
                Requests = requests,
                Time = TimeSpan.FromHours(hours)
            });
        }
    }
}
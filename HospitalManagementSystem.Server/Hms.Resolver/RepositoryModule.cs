namespace Hms.Resolver
{
    using System.Configuration;

    using Hms.Repositories;
    using Hms.Repositories.Interface;

    using Ninject;
    using Ninject.Modules;

    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Hms.Database"].ConnectionString;

            this.Bind<IGadgetKeysInfoRepository>().ToConstructor(_ => new GadgetKeysInfoRepository(connectionString));
            this.Bind<IUserRepository>().ToConstructor(_ => new UserRepository(connectionString));
            this.Bind<IUserRoleRepository>().ToConstructor(_ => new UserRoleRepository(connectionString));
            this.Bind<IMedicalCardRepository>().ToConstructor(_ => new MedicalCardRepository(connectionString));
            this.Bind<IProfileRepository>().ToConstructor(_ => new ProfileRepository(connectionString));
            this.Bind<IHealthcareInstitutionRepository>().ToConstructor(_ => new HealthcareInstitutionRepository(connectionString));
            this.Bind<IMedicalSpecializationRepository>().ToConstructor(_ => new MedicalSpecializationRepository(connectionString));
            this.Bind<IAttachmentRepository>().ToConstructor(_ => new AttachmentRepository(connectionString));
            this.Bind<IDoctorRepository>().ToConstructor(_ => new DoctorRepository(connectionString, KernelInstance.Get<IHealthcareInstitutionRepository>(), KernelInstance.Get<IMedicalSpecializationRepository>()));

            this.Bind<IPolyclinicRegionRepository>().ToConstructor(_ => new PolyclinicRegionRepository(connectionString, KernelInstance.Get<IDoctorRepository>()));
            this.Bind<IPolyclinicRepository>().ToConstructor(_ => new PolyclinicRepository(connectionString, KernelInstance.Get<IPolyclinicRegionRepository>(), this.KernelInstance.Get<IBuildingRepository>()));
            this.Bind<IBuildingRepository>().ToConstructor(_ => new BuildingRepository(connectionString, KernelInstance.Get<IPolyclinicRegionRepository>()));
        }
    }
}

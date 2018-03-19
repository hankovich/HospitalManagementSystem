namespace Hms.Resolver
{
    using System.Configuration;

    using Hms.Repositories;
    using Hms.Repositories.Interface;

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
            this.Bind<IPolyclinicRegionRepository>().ToConstructor(_ => new PolyclinicRegionRepository(connectionString));
            this.Bind<IBuildingRepository>().ToConstructor(_ => new BuildingRepository(connectionString));
        }
    }
}

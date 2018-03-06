﻿namespace Hms.Resolver
{
    using System.Configuration;

    using Hms.Repositories;
    using Hms.Repositories.Interface;

    using Ninject.Modules;

    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            this.Bind<IGadgetKeysInfoRepository>().ToConstructor(_ => new GadgetKeysInfoRepository(connectionString));
            this.Bind<IUserRepository>().ToConstructor(_ => new UserRepository(connectionString));
        }
    }
}
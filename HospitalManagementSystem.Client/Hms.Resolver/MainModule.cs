﻿namespace Hms.Resolver
{
    using Hms.Common;
    using Hms.Common.Interface;
    using Hms.Services;

    using Ninject.Modules;

    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IHttpContentService>().To<HttpContentService>();
            this.Bind<IAsymmetricCryptoProvider>().To<ElGamalCryptoProvider>();
            this.Bind<ISymmetricCryptoProvider>().To<AesCryptoProvider>();
        }
    }
}
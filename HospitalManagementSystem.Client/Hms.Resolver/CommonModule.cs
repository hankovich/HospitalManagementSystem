﻿namespace Hms.Resolver
{
    using Hms.Common;
    using Hms.Common.Geocoding;
    using Hms.Common.Interface;
    using Hms.Common.Interface.Geocoding;

    using Ninject.Modules;

    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IHttpContentService>().To<HttpContentService>();
            this.Bind<IAsymmetricCryptoProvider>().To<ElGamalCryptoProvider>();
            this.Bind<ISymmetricCryptoProvider>().To<AesCryptoProvider>();

            this.Bind<IGeoSuggester>().To<YandexSuggester>();
            this.Bind<IGeocoder>().To<YandexGeocoder>();
        }
    }
}
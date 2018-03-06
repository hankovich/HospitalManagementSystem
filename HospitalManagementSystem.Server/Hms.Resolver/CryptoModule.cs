namespace Hms.Resolver
{
    using Hms.Common;
    using Hms.Common.Interface;
    using Hms.Services;
    using Hms.Services.Interface;

    using Ninject.Modules;

    public class CryptoModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IHttpContentService>().To<HttpContentService>();
            this.Bind<IAuthenticationService>().To<AuthenticationService>();
            this.Bind<IAuthorizationService>().To<AuthorizationService>();

            this.Bind<ISymmetricCryptoProvider>().To<AesCryptoProvider>();
            this.Bind<IAsymmetricCryptoProvider>().To<ElGamalCryptoProvider>();
        }
    }
}

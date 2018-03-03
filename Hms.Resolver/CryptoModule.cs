namespace Hms.Resolver
{
    using Hms.Common;
    using Hms.Services;
    using Hms.Services.Interface;

    using Ninject.Modules;

    public class CryptoModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IHttpContentDecryptor>().To<HttpContentDecryptor>();
            this.Bind<IAuthenticationService>().To<AuthenticationService>();
            this.Bind<IAuthorizationService>().To<AuthorizationService>();
            this.Bind<IPrincipalService>().To<PrincipalService>();
            this.Bind<ISymmetricCryptoProvider>().To<AesCryptoProvider>();
        }
    }
}

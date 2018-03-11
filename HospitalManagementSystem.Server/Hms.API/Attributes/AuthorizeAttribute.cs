namespace Hms.API.Attributes
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Ninject;

    public class AuthorizeAttribute : AuthorizationFilterAttribute
    {
        public Role Roles { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public IAuthorizationService AuthorizationService { get; set; }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken token)
        {
            try
            {
                AuthenticationResult authenticationResult =
                    await
                    this.AuthenticationService.AuthenticateAsync(actionContext.Request.Headers.Authorization.Parameter);

                if (!authenticationResult.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException();
                }

                AuthorizationResult authorizationResult =
                    await this.AuthorizationService.AuthorizeAsync(authenticationResult.Principal?.Login, this.Roles);

                if (!authorizationResult.IsAuthorized)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                if (authenticationResult.Principal?.Login != null)
                {
                    actionContext.RequestContext.Principal =
                        new GenericPrincipal(new GenericIdentity(authenticationResult.Principal.Login), authorizationResult.AllRoles);
                }
            }
            catch
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
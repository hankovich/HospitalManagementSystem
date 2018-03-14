namespace Hms.API.Attributes
{
    using System;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    using Hms.API.Infrastructure;
    using Hms.Common.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Ninject;

    public class EncryptedAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public IHttpContentService HttpContentService { get; set; }

        private byte[] RoundKey { get; set; }

        public ExpirationPolicy Policy { get; set; } = ExpirationPolicy.Strong;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue authenticationHeader = context.ActionContext.Request.Headers.Authorization;
            AuthenticationResult authenticationResult =
                await this.AuthenticationService.AuthenticateAsync(authenticationHeader?.Parameter);

            if (!authenticationResult.IsAuthenticated)
            {
                context.ErrorResult = new HttpActionResult(
                    context.ActionContext.Request,
                    HttpStatusCode.Unauthorized, 
                    authenticationResult.FailureReason);
                return;
            }

            if (authenticationResult.IsRoundKeyExpired && this.Policy == ExpirationPolicy.Strong)
            {
                context.ErrorResult = new HttpActionResult(
                    context.ActionContext.Request,
                    (HttpStatusCode)424, 
                    authenticationResult.FailureReason);
                return;
            }

            if (authenticationResult.Principal != null)
            {
                context.Principal = new GenericPrincipal(
                    new GenericIdentity(authenticationResult.Principal.Login),
                    Array.Empty<string>());
            }

            this.RoundKey = authenticationResult.RoundKey;

            var content = context.ActionContext.Request?.Content;

            if (content == null || (content.Headers?.ContentLength ?? 0) == 0)
            {
                return;
            }

            context.ActionContext.Request.Content =
                await this.HttpContentService.DecryptAsync(content, authenticationResult.RoundKey);
        }

        public override async Task OnActionExecutedAsync(
            HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Response.Content == null || !actionExecutedContext.Response.IsSuccessStatusCode || this.RoundKey == null)
            {
                return;
            }

            actionExecutedContext.Response.Content =
                await this.HttpContentService.EncryptAsync(actionExecutedContext.Response.Content, this.RoundKey);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
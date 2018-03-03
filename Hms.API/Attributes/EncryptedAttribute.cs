namespace Hms.API.Attributes
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Ninject;

    public class EncryptedAttribute : FilterAttribute, IAuthenticationFilter
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public IHttpContentDecryptor HttpContentDecryptor { get; set; }

        [Inject]
        public IPrincipalService PrincipalService { get; set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (this.AuthenticationService == null || this.HttpContentDecryptor == null)
            {
                context.ActionContext.Response =
                    context.ActionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                return;
            }

            var content = context.ActionContext.Request?.Content;

            if (content == null)
            {
                return;
            }

            AuthenticationHeaderValue authenticationHeader = context.ActionContext.Request.Headers.Authorization;
            AuthenticationResult authenticationResult =
                await this.AuthenticationService.AuthenticateAsync(authenticationHeader?.Parameter);

            if (!authenticationResult.IsAuthenticated)
            {
                context.ActionContext.Response =
                    context.ActionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, authenticationResult.FailureReason);
                return;
            }

            if (authenticationResult.IsRoundKeyExpired)
            {
                context.ActionContext.Response =
                    context.ActionContext.Request.CreateErrorResponse(
                        HttpStatusCode.Unauthorized,
                        authenticationResult.FailureReason);
                return;
            }

            context.ActionContext.RequestContext.Principal =
                this.PrincipalService.ModelToPrincipal(authenticationResult.Principal);

            context.ActionContext.Request.Content =
                await this.HttpContentDecryptor.DecryptAsync(content, authenticationResult.Principal.RoundKey);
        }

        public Task ChallengeAsync(
            HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
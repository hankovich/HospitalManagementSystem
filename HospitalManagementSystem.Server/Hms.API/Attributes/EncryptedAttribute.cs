namespace Hms.API.Attributes
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;

    using Hms.Common.Interface;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using Ninject;

    public class EncryptedAttribute : FilterAttribute, IAuthenticationFilter
    {
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public IHttpContentService HttpContentService { get; set; }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var content = context.ActionContext.Request?.Content;

            if (content == null || content.Headers?.ContentLength == 0)
            {
                return;
            }

            AuthenticationHeaderValue authenticationHeader = context.ActionContext.Request.Headers.Authorization;
            AuthenticationResult authenticationResult =
                await this.AuthenticationService.AuthenticateAsync(authenticationHeader?.Parameter);

            if (!authenticationResult.IsAuthenticated)
            {
                context.ErrorResult = new UnauthorizedHttpActionResult(
                    context.ActionContext.Request,
                    authenticationResult.FailureReason);
                return;
            }

            if (authenticationResult.IsRoundKeyExpired)
            {
                context.ErrorResult = new UnauthorizedHttpActionResult(
                    context.ActionContext.Request,
                    authenticationResult.FailureReason);
                return;
            }

            context.ActionContext.Request.Content =
                await this.HttpContentService.DecryptAsync(content, authenticationResult.RoundKey);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private class UnauthorizedHttpActionResult : IHttpActionResult
        {
            private HttpRequestMessage Request { get; set; }

            private string Reason { get; set; }

            public UnauthorizedHttpActionResult(HttpRequestMessage request, string reason)
            {
                this.Request = request;
                this.Reason = reason;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(this.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, this.Reason));
            }
        }
    }
}
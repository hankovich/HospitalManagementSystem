namespace Hms.API.Infrastructure
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class HttpActionResult : IHttpActionResult
    {
        public HttpActionResult(HttpRequestMessage request, HttpStatusCode statusCode, string reason)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            this.Request = request;
            this.Reason = reason;
            this.StatusCode = statusCode;
        }

        public HttpActionResult(ApiController controller, HttpStatusCode statusCode, string reason)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (controller.Request == null)
            {
                throw new ArgumentNullException(nameof(controller.Request));
            }

            this.Request = controller.Request;
            this.Reason = reason;
            this.StatusCode = statusCode;
        }

        private HttpRequestMessage Request { get; }

        private string Reason { get; }

        private HttpStatusCode StatusCode { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Request.CreateErrorResponse(this.StatusCode, this.Reason ?? string.Empty));
        }
    }
}
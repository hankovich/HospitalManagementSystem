namespace Hms.API.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    [RoutePrefix("api/attachment")]
    public class AttachmentController : ApiController
    {
        public AttachmentController(IAttachmentService attachmentService)
        {
            this.AttachmentService = attachmentService;
        }

        public IAttachmentService AttachmentService { get; }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [Route("{attachmentId}")]
        public async Task<IHttpActionResult> GetAttachment(int attachmentId)
        {
            try
            {
                return this.Ok(await this.AttachmentService.GetAttachmentAsync(this.User.Identity.Name, attachmentId));
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [Route("info/{attachmentId}")]
        public async Task<IHttpActionResult> GetAttachmentInfo(int attachmentId)
        {
            try
            {
                return this.Ok(await this.AttachmentService.GetAttachmentInfoAsync(this.User.Identity.Name, attachmentId));
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
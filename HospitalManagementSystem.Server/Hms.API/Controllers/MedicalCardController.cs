namespace Hms.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    using AuthorizeAttribute = Hms.API.Attributes.AuthorizeAttribute;

    [RoutePrefix("api/card")]
    public class MedicalCardController : ApiController
    {
        public MedicalCardController(IMedicalCardService medicalCardService)
        {
            if (medicalCardService == null)
            {
                throw new ArgumentNullException(nameof(medicalCardService));
            }

            this.MedicalCardService = medicalCardService;
        }

        public IMedicalCardService MedicalCardService { get; }

        [Encrypted, Authorize(Roles = Role.Patient)]
        [HttpGet, Route("{pageIndex}/{pageSize}/{filter?}")]
        public async Task<IHttpActionResult> Get(int pageIndex, int pageSize, string filter = "")
        {
            try
            {
                MedicalCard card = await this.MedicalCardService.GetMedicalCardPagesAsync(this.User.Identity.Name, pageIndex, pageSize, filter);
                return this.Ok(card);
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}

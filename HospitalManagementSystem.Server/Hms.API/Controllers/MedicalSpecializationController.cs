namespace Hms.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    [RoutePrefix("api/specialization")]
    public class MedicalSpecializationController : ApiController
    {
        public MedicalSpecializationController(IMedicalSpecializationService medicalSpecializationService)
        {
            this.MedicalSpecializationService = medicalSpecializationService;
        }

        public IMedicalSpecializationService MedicalSpecializationService { get; }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [HttpGet, Route("{institutionId}/{pageIndex}/{pageSize}/{filter?}")]
        public async Task<IHttpActionResult> Get(int institutionId, int pageIndex, int pageSize, string filter = "")
        {
            try
            {
                IEnumerable<MedicalSpecialization> specializations = await this.MedicalSpecializationService.GetSpecializationsAsync(institutionId, pageIndex, pageSize, filter);
                return this.Ok(specializations);
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [HttpGet, Route("count/{institutionId}/{filter?}")]
        public async Task<IHttpActionResult> GetCount(int institutionId, string filter = "")
        {
            try
            {
                int specializationsCount = await this.MedicalSpecializationService.GetSpecializationsCountAsync(institutionId, filter);
                return this.Ok(specializationsCount);
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [Route("{specializationId}")]
        public async Task<IHttpActionResult> GetSpecialization(int specializationId)
        {
            try
            {
                return this.Ok(await this.MedicalSpecializationService.GetSpecializationAsync(specializationId));
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}

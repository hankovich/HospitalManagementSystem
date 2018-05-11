namespace Hms.API.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Services.Interface;
    using Hms.Services.Interface.Models;

    [RoutePrefix("api/doctor")]
    public class DoctorController : ApiController
    {
        public DoctorController(IDoctorService doctorService)
        {
            this.DoctorService = doctorService;
        }

        public IDoctorService DoctorService { get; }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [Route("{doctorId}"), HttpGet]
        public async Task<IHttpActionResult> Get(int doctorId)
        {
            try
            {
                return this.Ok(await this.DoctorService.GetDoctorAsync(doctorId));
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [Route("{polyclinicId}/{specializationId}/{pageIndex}/{pageSize}/{filter?}"), HttpGet]
        public async Task<IHttpActionResult> Get(int polyclinicId, int specializationId, int pageIndex, int pageSize, string filter = "")
        {
            try
            {
                return this.Ok(await this.DoctorService.GetDoctorsAsync(polyclinicId, specializationId, pageIndex, pageSize, filter));
            }
            catch
            {
                return this.BadRequest();
            }
        }

        [Encrypted, Attributes.Authorize(Roles = Role.Patient)]
        [HttpGet, Route("count/{polyclinicId}/{specializationId}/{filter?}")]
        public async Task<IHttpActionResult> GetCount(int polyclinicId, int specializationId, string filter = "")
        {
            try
            {
                int doctorsCount = await this.DoctorService.GetDoctorsCountAsync(polyclinicId, specializationId, filter);
                return this.Ok(doctorsCount);
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}

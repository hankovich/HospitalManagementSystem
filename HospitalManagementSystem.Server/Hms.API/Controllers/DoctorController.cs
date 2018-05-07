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
    }
}

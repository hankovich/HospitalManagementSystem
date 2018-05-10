namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.API.Infrastructure;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    [RoutePrefix("api/polyclinic")]
    public class PolyclinicController : ApiController
    {
        public PolyclinicController(IPolyclinicService polyclinicService)
        {
            this.PolyclinicService = polyclinicService;
        }

        public IPolyclinicService PolyclinicService { get; }

        [Encrypted]
        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Polyclinic polyclinic = await this.PolyclinicService.GetPolyclinicAsync(id);

                return this.Ok(polyclinic);
            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Encrypted]
        [HttpPost, Route]
        public async Task<IHttpActionResult> Post([FromBody] Polyclinic polyclinic)
        {
            try
            {
                if (polyclinic == null)
                {
                    throw new ArgumentNullException($"{nameof(polyclinic)} must be not null");
                }

                int id = await this.PolyclinicService.InsertOrUpdatePolyclinicAsync(polyclinic);

                return this.Ok(id);
            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
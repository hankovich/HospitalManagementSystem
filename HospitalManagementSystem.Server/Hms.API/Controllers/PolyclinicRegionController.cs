namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.API.Infrastructure;
    using Hms.Common.Interface.Domain;

    [RoutePrefix("api/region")]
    public class PolyclinicRegionController : ApiController
    {
        public PolyclinicRegionController(IPolyclinicRegionService polyclinicRegionService)
        {
            this.PolyclinicRegionService = polyclinicRegionService;
        }

        public IPolyclinicRegionService PolyclinicRegionService { get; set; }

        [Encrypted]
        [HttpGet, Route("{id")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                PolyclinicRegion region = await this.PolyclinicRegionService.GetRegionAsync(id);

                return this.Ok(region);
            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Infrastructure;

    [RoutePrefix("api/building")]
    public class BuildingController : ApiController
    {
        public BuildingController(IBuildingService buildingService)
        {
            this.BuildingService = buildingService;
        }

        public IBuildingService BuildingService { get; set; }

        [Route("{latitude}/{longitude}")]
        public async Task<IHttpActionResult> Get(int latitude, int longitude)
        {
            try
            {

            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

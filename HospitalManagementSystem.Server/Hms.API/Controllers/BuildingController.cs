namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.API.Infrastructure;
    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Geocoding;
    using Hms.Services.Interface;

    [RoutePrefix("api/building")]
    public class BuildingController : ApiController
    {
        public BuildingController(IBuildingService buildingService)
        {
            this.BuildingService = buildingService;
        }

        public IBuildingService BuildingService { get; set; }

        [Encrypted]
        [HttpGet, Route("{latitude}/{longitude}")]
        public async Task<IHttpActionResult> Get(double latitude, double longitude)
        {
            try
            {
                BuildingAddress building = await this.BuildingService.GetBuildingAsync(new GeoPoint(longitude, latitude));

                return this.Ok(building);
            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

﻿namespace Hms.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.API.Infrastructure;
    using Hms.Common.Interface.Domain;
    using Hms.Services.Interface;

    [RoutePrefix("api/region")]
    public class PolyclinicRegionController : ApiController
    {
        public PolyclinicRegionController(IPolyclinicRegionService polyclinicRegionService)
        {
            this.PolyclinicRegionService = polyclinicRegionService;
        }

        public IPolyclinicRegionService PolyclinicRegionService { get; }

        [Encrypted]
        [HttpGet, Route("{id}")]
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

        [Encrypted]
        [HttpPost, Route]
        public async Task<IHttpActionResult> Post([FromBody] PolyclinicRegion polyclinicRegion)
        {
            try
            {
                if (polyclinicRegion == null)
                {
                    throw new ArgumentNullException($"{nameof(polyclinicRegion)} must be not null");    
                }

                int id = await this.PolyclinicRegionService.InsertOrUpdateRegionAsync(polyclinicRegion);

                return this.Ok(id);
            }
            catch (Exception e)
            {
                return new HttpActionResult(this, HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

namespace Hms.API.Controllers
{
    using System.Web.Http;

    using Hms.API.Attributes;
    using Hms.Services.Interface.Models;

    using AuthorizeAttribute = Hms.API.Attributes.AuthorizeAttribute;

    [RoutePrefix("api/hello")]
    public class HelloController : ApiController
    {
        [Encrypted, Authorize(Roles = Role.Doctor | Role.Patient)]
        [HttpGet]
        [Route("get")]
        public int Get([FromBody]int a)
        {
            return 4;
        }
    }
}

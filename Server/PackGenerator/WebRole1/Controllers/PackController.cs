using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace WebRole.Controllers
{
    public class PackController : ApiController
    {
        // Get a pack
        // GET api/pack/akh
        [SwaggerOperation("GetBySet")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public string Get(string set)
        {
            return $"pack from {set}";
        }
    }
}

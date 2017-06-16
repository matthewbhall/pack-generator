using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using WebRole.Managers;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class PackController : ApiController
    {
        private Gatherer gatherer = Gatherer.Instance();

        // Get a pack
        // GET api/pack/akh
        [SwaggerOperation("GetBySetName")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public List<Card> Get(string setName)
        {
            var set = gatherer.GetSet(setName);
            if (set == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return gatherer.PackManager.GeneratePack(set);
        }
    }
}

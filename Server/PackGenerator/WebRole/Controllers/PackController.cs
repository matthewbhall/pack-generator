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
    /// <summary>
    /// PackController is a web API entry point to generating booster packs.
    /// </summary>
    public class PackController : ApiController
    {
        private Gatherer gatherer = Gatherer.Instance();

        /// <summary>
        /// Generates a random pack of the specified set and returns the pack
        /// as a list of card objects.
        /// 
        /// Usage: GET api/pack/{set}
        /// </summary>
        /// <param name="setName">Name of the set to generate a pack from.</param>
        /// <returns>A randomly generated pack.</returns>
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

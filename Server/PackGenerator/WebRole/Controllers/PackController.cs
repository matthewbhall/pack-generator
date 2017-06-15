using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using WebRole.Managers;

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
            var gatherer = Gatherer.Instance();
            SetManager manager;
            gatherer.SetManagers.TryGetValue(set, out manager);
            return JsonConvert.SerializeObject(manager.GetMythics(), Formatting.Indented);
        }
    }
}

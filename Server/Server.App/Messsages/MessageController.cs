using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Linq;

namespace Server.App.Messsages
{
    [EnableCors(origins: "http://localhost:54842", headers: "*", methods: "*")]
    [RoutePrefix("messages")]
    public class MessageController : ApiController
    {
        private static string _theMessage = "Hi Mum";

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return _theMessage;
        }

        [HttpGet]
        [Route("{count:int:min(1)?}")]
        public IEnumerable<string> Get(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                yield return _theMessage;
            }
        }

        [HttpPost]
        [Route("{message}")]
        public IHttpActionResult PostWithDataFromUrl(string message)
        {
            _theMessage = message;
            return Ok(_theMessage);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostWithDataFromBody([FromBody]JObject data)
        {
            JToken message;
            if (data.TryGetValue("message", out message))
            {
                _theMessage = message.Value<string>();
            }
            return Ok(_theMessage);
        }
    }
}

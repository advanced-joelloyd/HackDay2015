using System.Collections.Generic;
using System.Web.Http;

namespace Server.App.Messsages
{
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
    }
}

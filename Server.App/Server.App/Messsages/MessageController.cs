using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Server.App.Messsages
{
    [RoutePrefix("messages")]
    public class MessageController : ApiController
    {
        private const string TheMessage = "Hi Mum";

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return TheMessage;
        }

        [HttpGet]
        [Route("{count:int:min(1)?}")]
        public IEnumerable<string> Get(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                yield return TheMessage;
            }
        }
    }
}

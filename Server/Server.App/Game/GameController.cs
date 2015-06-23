using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;

namespace Server.App.Game
{
    public class GameController : ApiController
    {
        private static Game _game;

        [Route("start")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] FormDataCollection data, int start = 1)
        {
            if (_game != null)
            {
                _game.Persist();
            }

            _game = new Game
            {
                OpponentName = data.Get("opponentName"),
                PointsToWin = int.Parse(data.Get("pointsToWin")),
                MaxRounds = int.Parse(data.Get("maxRounds")),
                DynamiteCount = int.Parse(data.Get("dynamiteCount"))
            };

            return Ok("New game started");
        }

        [Route("move")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(_game.GetOurNextMove(), Encoding.UTF8, "text/html")
                };
            }
            catch
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(Move.Paper, Encoding.UTF8, "text/html")
                };
            }
        }

        [Route("move")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] FormDataCollection data)
        {
            var lastOpponentMove = data.Get("lastOpponentMove");
            _game.OpponentsMove(lastOpponentMove);

            return Ok("Move recorded");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.App.Game
{
    public class Round
    {
        public Guid GameId { get; set; }

        public string OurMove { get; set; }

        public string TheirMove { get; set; }

        public int Result
        {
            get
            {
                if (OurMove == TheirMove)
                {
                    return 0;
                }
                else if (OurMove == Move.Waterbomb && TheirMove == Move.Dynamite)
                {
                    return 1;
                }
                else if (OurMove == Move.Waterbomb && TheirMove != Move.Dynamite)
                {
                    return -1;
                }
                else if (OurMove == Move.Dynamite && TheirMove == Move.Waterbomb)
                {
                    return -1;
                }
                else if (OurMove == Move.Dynamite && TheirMove != Move.Waterbomb)
                {
                    return 1;
                }
                else if (OurMove == Move.Rock && TheirMove != Move.Paper)
                {
                    return 1;
                }
                else if (OurMove == Move.Paper && TheirMove != Move.Scissos)
                {
                    return 1;
                }
                else if (OurMove == Move.Scissos && TheirMove != Move.Rock)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
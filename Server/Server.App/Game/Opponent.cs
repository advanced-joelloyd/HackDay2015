using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.App.Game
{
    public class Opponent
    {
        public Opponent()
        {
            this.Moves = new List<string>();
        }

        public string Name { get; set; }

        public List<string> Moves { get; set; }
    }
}
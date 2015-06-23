using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.App.Entities;

namespace Server.App.Game
{
    public class Game
    {
        private int _dynamiteCount;

        public Game()
        {
            Opponent = new Opponent();
            PreviousRounds = new List<Round>();
        }

        public void Persist()
        {
            try
            {
                PersistInternal();
            }
            catch (Exception)
            {
            }
        }

        private void PersistInternal()
        {
            using (var context = new HackDayEntities1())
            {
                var lastGame = context.Games.OrderByDescending(g => g.Timestamp).FirstOrDefault();

                var game = new Entities.Game
                {
                    ID = Guid.NewGuid(),
                    OpponentName = this.Opponent.Name,
                    MaxRounds = this.MaxRounds,
                    PointsToWin = this.PointsToWin,
                    DynamiteCount = this.DynamiteCount,
                    Timestamp = DateTime.Now,
                    GameNumber = lastGame != null  ? lastGame.GameNumber + 1 : 0
                };

                var r = 0;
                foreach (var round in this.PreviousRounds)
                {
                    var roundEntity = new Entities.Round
                    {
                        ID = Guid.NewGuid(),
                        Timestamp = DateTime.Now,
                        OurMove = round.OurMove,
                        TheirMove = round.TheirMove,
                        RoundNumber = r++
                    };
                    game.Rounds.Add(roundEntity);
                }

                context.Games.Add(game);
                context.SaveChanges();
            }
        }

        public Guid GameID { get; set; }
        public Opponent Opponent { get; set; }
        public int PointsToWin { get; set; }
        public int CurrentWins { get; set; }
        public int MaxRounds { get; set; }

        public int DynamiteCount
        {
            get { return _dynamiteCount; }
            set
            {
                CurrentDynamiteCount = value;
                _dynamiteCount = value;
            }
        }

        public int CurrentDynamiteCount { get; set; }
        public int CurrentWaterbombUses { get; set; }

        public List<Round> PreviousRounds { get; set; }
        public Round CurrentRound { get; set; }

        public string GetOurNextMove()
        {
            if (CurrentRound == null)
            {
                CurrentRound = new Round();
            }

            var ourMove = OurMove();

            CurrentRound.OurMove = ourMove;
            return ourMove;
        }

        private string OurMove()
        {
            if (CurrentRound == null)
            {
                CurrentRound = new Round();
            }

            string ourMove = Move.Paper;

            if (Opponent.Name == "FATBOTSLIM")
            {
                ourMove = GetFATBOTCounter();
            }
            else
            {
                if (CurrentWins  < PreviousRounds.Count() + 10 && PreviousRounds.Count() % 10 == 0)
                {
                    ourMove = GetRandomMove();
                }
                else
                {
                    ourMove = GetDefaultMove(PreviousRounds.Last());
                }
            }

            CurrentRound.OurMove = ourMove;
            return ourMove;
        }

        private string GetRandomMove()
        {
            var random = new Random().Next(1, 3);
            switch (random)
            {
                case 1:
                    return Move.Paper;
                    break;
                case 2:
                    return Move.Rock;
                    break;
                case 3:
                    return Move.Scissos;
                    break;
                default:
                    return Move.Paper;
                    break;
            }
        }

        private string GetFATBOTCounter()
        {
            var previousRound = PreviousRounds.Last();

            var ourMove = GetDefaultMove(PreviousRounds.Last());

            if (previousRound.Result == 0 && this.CurrentWaterbombUses < this.DynamiteCount)
            {
                ourMove = Move.Waterbomb;
                this.CurrentWaterbombUses++;
            }
            else if (previousRound.Result == 0)
            {
                ourMove = previousRound.OurMove;
            }

            return ourMove;
        }

        private string GetDefaultMove(Round previousRound)
        {
            var ourMove = Move.Paper;

            if (previousRound.Result == 0 || previousRound.Result == -1)
            {
                ourMove = GetCounter(previousRound);
            }
            else
            {
                if (previousRound.OurMove == Move.Dynamite)
                {
                    ourMove = Move.Rock;
                }
                else
                {
                    ourMove = previousRound.OurMove;
                }
            }
            if (ShouldWeThrowDyamite())
            {
                ourMove = ThrowDyamite();
            }

            return ourMove;
        }

        private static string GetCounter(Round previousRound)
        {
            switch (previousRound.TheirMove)
            {
                case Move.Rock:
                    return Move.Paper;
                    break;
                case Move.Paper:
                    return Move.Scissos;
                    break;
                case Move.Scissos:
                    return Move.Rock;
                    break;
                case Move.Dynamite:
                    return Move.Rock;
                    break;
                case Move.Waterbomb:
                    return Move.Rock;
                    break;
                default:
                    return Move.Rock;
                    break;
            }
        }


        private bool ShouldWeThrowDyamite()
        {
            if (CurrentDynamiteCount <= 0)
            {
                return false;
            }

            if (PreviousRounds.Count % 5 == 0)
            {
                return true;
            }

            return false;
        }

        private string ThrowDyamite()
        {
            CurrentDynamiteCount--;
            return Move.Dynamite;
        }

        public void OpponentsMove(string lastOpponentMove)
        {
            if (CurrentRound == null)
            {
                CurrentRound = new Round();
            }

            CurrentRound.TheirMove = lastOpponentMove;
            PreviousRounds.Add(CurrentRound);

            if (CurrentRound.Result == 1)
            {
                CurrentWins++;
            }

            CurrentRound = new Round();
        }
    }
}
using FantasyEsportsBattle.Web.Data.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Extensions
{
    public static class PlayerExtensions
    {
        public const float StartingPoints = 20;
        public static float CalculatePoints(this CompetitionPlayer competitionPlayer, Tournament tournament)
        {
            switch (tournament.TournamentAlgorithm)
            {
                case Enumerations.TournamentAlgorithm.Standard:
                {
                    //competitionPlayer.
                    break;
                }
            }

            return 0;
        }
    }
}

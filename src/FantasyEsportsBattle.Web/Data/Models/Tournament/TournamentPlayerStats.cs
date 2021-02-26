using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class TournamentPlayerStats
    {
        public int Id { get; set; }
        public int TournamentPlayerId { get; set; }
        public float Winrate { get; set; }
        public TournamentPlayer TournamentPlayer { get; set; }
    }
}

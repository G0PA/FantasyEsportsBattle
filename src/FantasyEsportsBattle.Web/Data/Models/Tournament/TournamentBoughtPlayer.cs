using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class TournamentBoughtPlayer
    {
        public int Id { get; set; }

        public int TournamentStatsId { get; set; }

        public virtual TournamentStats TournamentStats { get; set; }

        public int CompetitionPlayerId { get; set; }

        public virtual CompetitionPlayer CompetitionPlayer { get; set; }
        public bool IsReserve { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class TournamentCompetition
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
    }
}

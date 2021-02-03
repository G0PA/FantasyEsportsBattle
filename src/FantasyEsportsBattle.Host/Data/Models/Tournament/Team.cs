using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Host.Data.Models.Tournament
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TournamentPlayer> Players { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
    }
}

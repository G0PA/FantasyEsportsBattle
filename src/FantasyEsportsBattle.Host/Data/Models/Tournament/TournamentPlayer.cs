using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Host.Data.Models.Tournament
{
    public class TournamentPlayer
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int DisplayImage { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public TournamentPlayerStats TournamentPlayerStats { get; set; }
    }
}

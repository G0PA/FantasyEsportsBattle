using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class TournamentPlayer
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int DisplayImage { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual TournamentPlayerStats TournamentPlayerStats { get; set; }
    }
}

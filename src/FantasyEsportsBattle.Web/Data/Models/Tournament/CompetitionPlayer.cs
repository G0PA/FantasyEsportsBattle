using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Web.Enumerations;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class CompetitionPlayer
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public Roles Role { get; set; }
        public int DisplayImage { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int CompetitionPlayerStatsId { get; set; }
        public virtual CompetitionPlayerStats CompetitionPlayerStats { get; set; }
    }
}

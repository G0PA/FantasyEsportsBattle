using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Host.Enumerations;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Region Region { get; set; }
        public int DisplayImage { get; set; }
        public ICollection<Team> Teams;
        public int Priority { get; set; }
        public ICollection<TournamentCompetition> TournamentCompetitions { get; set; }
    }
}

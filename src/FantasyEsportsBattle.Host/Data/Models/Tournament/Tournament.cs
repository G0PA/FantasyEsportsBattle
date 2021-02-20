using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Host.Data.Models.Tournament
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int DisplayImage { get; set; }
        public int MaxParticipants { get; set; }
        public string Description { get; set; }
        public ICollection<TournamentCompetition> TournamentCompetitions { get; set; }
        public ICollection<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
        public int Priority { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Enumerations;

namespace FantasyEsportsBattle.Models.Tournament
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int DisplayImage { get; set; }
        public int MaxParticipants { get; set; }
        public float StartingCurrency { get; set; }
        public virtual ICollection<TournamentCompetition> TournamentCompetitions { get; set; }
        public virtual ICollection<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
        public virtual ICollection<TournamentInvitation> TournamentInvitations { get; set; }
        public string TournamentHostId { get; set; }
        public virtual ApplicationUser TournamentHost { get; set; }
        public int Priority { get; set; }
        public TournamentType TournamentType { get; set; }
        public TournamentAlgorithm TournamentAlgorithm { get; set; }
        public TournamentState TournamentState { get; set; }
    }
}

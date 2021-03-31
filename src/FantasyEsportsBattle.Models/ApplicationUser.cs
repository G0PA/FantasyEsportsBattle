using System;
using System.Collections.Generic;
using FantasyEsportsBattle.Models.Tournament;
using FantasyEsportsBattle.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace FantasyEsportsBattle.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
        public virtual ICollection<TournamentInvitation> TournamentInvitations { get; set; }
        public virtual ICollection<Tournament.Tournament> HostedTournaments { get; set; }
        public virtual ICollection<TournamentStats> TournamentStatuses { get; set; }
        public AccountType AccountType { get; set; }
    }
}

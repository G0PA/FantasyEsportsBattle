using System.Collections.Generic;
using FantasyEsportsBattle.Host.Data.Models.Tournament;
using Microsoft.AspNetCore.Identity;

namespace FantasyEsportsBattle.Host.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Web.Data.Models.Tournament;
using Microsoft.AspNetCore.Identity;

namespace FantasyEsportsBattle.Web.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserTournament> ApplicationUserTournaments { get; set; }
    }
}

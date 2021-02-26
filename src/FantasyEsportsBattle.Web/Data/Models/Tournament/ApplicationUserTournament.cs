using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class ApplicationUserTournament
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}

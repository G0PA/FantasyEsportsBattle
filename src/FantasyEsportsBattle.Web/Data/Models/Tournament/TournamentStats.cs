using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class TournamentStats
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public float Currency { get; set; }
        public virtual ICollection<TournamentBoughtPlayer> BoughtPlayers { get; set; }
    }
}

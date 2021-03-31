using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Models.Tournament
{
    public class TournamentInvitation
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
        public string InvitedUserId { get; set; }
        public virtual ApplicationUser InvitedUser { get; set; }
        public string InvitationSenderUserId { get; set; }
        public virtual ApplicationUser InvitationSenderUser { get; set; }
    }
}

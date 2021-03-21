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
        public Role Role { get; set; }
        public int DisplayImage { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int CompetitionPlayerStatsId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public float Winrate { get; set; }
        public float KDA { get; set; }
        public float CSPM { get; set; }
        public float GPM { get; set; }
        public float GoldPercent { get; set; }
        public float KillParticipationPercent { get; set; }
        public float AheadInCSAt15MinPercent { get; set; }
        public float CSDifferenceAt15Min { get; set; }
        public float GoldDifferenceAt15Min { get; set; }
        public float XPDifferenceAt15Min { get; set; }
        public float DamagePerMinute { get; set; }
        public float DamagePercent { get; set; }
        public float VisionScorePerMinute { get; set; }
        public float FirstBloodParticipationPercent { get; set; }
        public float FirstBloodVictimPercent { get; set; }
        public virtual CompetitionPlayerStats CompetitionPlayerStats { get; set; }

        public override string ToString()
        {
            return $"{Nickname}";
        }
    }
}

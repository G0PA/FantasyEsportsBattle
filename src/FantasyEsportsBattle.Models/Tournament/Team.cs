using System.Collections.Generic;
using FantasyEsportsBattle.Enumerations;

namespace FantasyEsportsBattle.Models.Tournament
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CompetitionPlayer> Players { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public int DisplayImage { get; set; }
        public virtual Region Region { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalGames { get; set; }
        public float Winrate { get; set; }
        public float AverageGameTime { get; set; }
    }
}

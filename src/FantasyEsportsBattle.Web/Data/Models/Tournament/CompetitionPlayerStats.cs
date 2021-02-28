namespace FantasyEsportsBattle.Web.Data.Models.Tournament
{
    public class CompetitionPlayerStats
    {
        public int Id { get; set; }
        public int TournamentPlayerId { get; set; }
        public float Winrate { get; set; }
        public int CompetitionPlayerId { get; set; }
        public virtual CompetitionPlayer CompetitionPlayer { get; set; }
    }
}

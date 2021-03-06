﻿using System;

namespace FantasyEsportsBattle.Models.Tournament
{
    public class FinishedEvent
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public virtual DateTime GameDate { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
    }
}

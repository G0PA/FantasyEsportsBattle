using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Host.Data.Models.Tournament;

namespace FantasyEsportsBattle.Host.DTOs
{
    public class TournamentInfo
    {
        public List<Competition> Competitions { get; set; }
    }
}

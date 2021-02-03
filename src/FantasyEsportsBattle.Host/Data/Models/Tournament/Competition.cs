using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Host.Enumerations;

namespace FantasyEsportsBattle.Host.Data.Models.Tournament
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Region Region { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<Team> Teams;
        public int Priority { get; set; }
    }
}

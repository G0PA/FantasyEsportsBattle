using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyEsportsBattle.Web.Data;
using FantasyEsportsBattle.Web.Data.Models.Tournament;

namespace FantasyEsportsBattle.Web.Services
{
    public class ExpiredCompetitionsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpiredCompetitionsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Competition> GetExpiredCompetitions(List<Competition> competitions)
        {
            var expired = new List<Competition>();

            var competitionsInDb = _dbContext.Competitions;

            foreach (var competition in competitionsInDb)
            {
                if (!competitions.Contains(competition))
                {
                    expired.Add(competition);
                }
            }

            return expired;
        }
    }
}

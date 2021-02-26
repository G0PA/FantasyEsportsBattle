using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using FantasyEsportsBattle.Web.Data;
using FantasyEsportsBattle.Web.Data.Models;
using FantasyEsportsBattle.Web.Data.Models.Tournament;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FantasyEsportsBattle.Web.Areas.Identity.Pages.Tournaments
{
    public class TournamentCreation
    {
        private readonly ILogger<TournamentCreation> _logger;
        private readonly ApplicationDbContext _dbContext;

        public TournamentCreation(ILogger<TournamentCreation> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public bool OnCreateTournament(List<string> checkedCompetitions, string tournamentName, int tournamentSize,ClaimsPrincipal claims)
        {
            if (_dbContext.Tournaments.Any(c => c.Name == tournamentName))
            {
                return false;
            }

            _dbContext.Tournaments.Add(new Tournament
            {
                Name = tournamentName,
                MaxParticipants = tournamentSize,
            });

            _dbContext.SaveChanges();

            var tournamentId = _dbContext.Tournaments.FirstOrDefault(t => t.Name == tournamentName).Id;

            foreach (var competition in checkedCompetitions)
            {
                var competitionFromDb = _dbContext.Competitions.FirstOrDefault(c => c.Name == competition);

                _dbContext.TournamentCompetitions.Add(new TournamentCompetition
                {
                    CompetitionId = competitionFromDb.Id,
                    TournamentId = tournamentId
                });
            }

            _dbContext.ApplicationUserTournaments.Add(new ApplicationUserTournament
                {TournamentId = tournamentId, ApplicationUserId = claims.Claims.FirstOrDefault().Value});

            _dbContext.SaveChanges();

            return true;
        }
    }
}

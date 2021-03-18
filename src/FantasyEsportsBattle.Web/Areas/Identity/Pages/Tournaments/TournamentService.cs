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
using FantasyEsportsBattle.Web.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FantasyEsportsBattle.Web.Areas.Identity.Pages.Tournaments
{
    public class TournamentService
    {
        private readonly ILogger<TournamentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public TournamentService(ILogger<TournamentService> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public bool OnCreateTournament(List<string> checkedCompetitions, string tournamentName, int tournamentSize,ClaimsPrincipal claims, TournamentType tournamentType,TournamentAlgorithm tournamentAlgorithm, float currency)
        {
            if (_dbContext.Tournaments.Any(c => c.Name == tournamentName))
            {
                return false;
            }

            var id = claims.Claims.FirstOrDefault().Value;

            _dbContext.Tournaments.Add(new Tournament
            {
                Name = tournamentName,
                MaxParticipants = tournamentSize,
                TournamentHostId = id,
                TournamentType = tournamentType,
                TournamentAlgorithm = tournamentAlgorithm,
                StartingCurrency = currency,
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

            _dbContext.SaveChanges();

            JoinTournament(_dbContext.Tournaments.FirstOrDefault(t => t.Id == tournamentId), _dbContext.Users.FirstOrDefault(u => u.Id == id));

            return true;
        }

        public bool JoinTournament(Tournament tournament, ApplicationUser applicationUser)
        {
            if (tournament.TournamentState != TournamentState.NotStarted || 
                (tournament.ApplicationUserTournaments != null &&
                tournament.MaxParticipants <= tournament.ApplicationUserTournaments.Count))
            {
                return false;
            }

            _dbContext.ApplicationUserTournaments.Add(new ApplicationUserTournament
            { Tournament = tournament, ApplicationUser = applicationUser });

            _dbContext.TournamentStatuses.Add(new TournamentStats
            {
                ApplicationUser = applicationUser,
                Tournament = tournament,
                Currency = tournament.StartingCurrency
            });

            _dbContext.SaveChanges();

            return true;
        }

        public bool OnAcceptInvitation(TournamentInvitation tournamentInvitation)
        {
            if (JoinTournament(tournamentInvitation.Tournament, tournamentInvitation.InvitedUser))
            {

                _dbContext.TournamentInvitations.Remove(tournamentInvitation);

                _dbContext.SaveChanges();

                return true;
            }

            return false;
        }
    }
}

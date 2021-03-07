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

        public bool OnCreateTournament(List<string> checkedCompetitions, string tournamentName, int tournamentSize,ClaimsPrincipal claims, TournamentType tournamentType,TournamentAlgorithm tournamentAlgorithm)
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
                TournamentAlgorithm = tournamentAlgorithm
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
                {TournamentId = tournamentId, ApplicationUserId = id });

            _dbContext.SaveChanges();

            return true;
        }

        public bool OnAcceptInvitation(TournamentInvitation tournamentInvitation)
        {
            if(tournamentInvitation.Tournament.TournamentState != TournamentState.NotStarted || tournamentInvitation.Tournament.MaxParticipants <= tournamentInvitation.Tournament.ApplicationUserTournaments.Count)
            {
                return false;
            }

            tournamentInvitation.Tournament.ApplicationUserTournaments.Add(new ApplicationUserTournament
            {
                ApplicationUser = tournamentInvitation.InvitedUser,
                Tournament = tournamentInvitation.Tournament
            });

            _dbContext.TournamentInvitations.Remove(tournamentInvitation);

            _dbContext.SaveChanges();

            return true;
        }
    }
}

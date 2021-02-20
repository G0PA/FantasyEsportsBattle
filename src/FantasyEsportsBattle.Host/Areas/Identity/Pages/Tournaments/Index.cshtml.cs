using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FantasyEsportsBattle.Data;
using FantasyEsportsBattle.Host.Data.Models.Tournament;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FantasyEsportsBattle.Host.Areas.Identity.Pages.Tournaments
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _dbContext;
        public List<string> Competitions;

        [BindProperty]
        public List<string> CheckedCompetitions { get; set; }
        [BindProperty]
        public string TournamentName { get; set; }

        public IndexModel(ILogger<CreateModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            Competitions = dbContext.Competitions.Select(c => c.Name).ToList();
        }

        public void OnGet()
        {

        }

        public ActionResult OnPostCreateTournament()
        {
            if (_dbContext.Competitions.Any(c => c.Name == TournamentName) || Request.HttpContext?.User == null)
            {
                return StatusCode((int) HttpStatusCode.BadRequest,"Tournament Name Already In Use");
            }

            _dbContext.Tournaments.Add(new Tournament
            {
                Name = TournamentName,
            });

            _dbContext.SaveChanges();

            var tournamentId = _dbContext.Tournaments.FirstOrDefault(t => t.Name == TournamentName).Id;

            foreach (var competition in CheckedCompetitions)
            {
                var competitionFromDb = _dbContext.Competitions.FirstOrDefault(c => c.Name == competition);
                _dbContext.TournamentCompetitions.Add(new TournamentCompetition
                {
                    CompetitionId = competitionFromDb.Id,
                    TournamentId = tournamentId
                });
            }

            _dbContext.ApplicationUserTournaments.Add(new ApplicationUserTournament
                {TournamentId = tournamentId, ApplicationUserId = Request.HttpContext.User.Claims.FirstOrDefault().Value});

            _dbContext.SaveChanges();

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net;
<<<<<<< HEAD
=======
using System.Net.Mime;
using System.Threading.Tasks;
>>>>>>> master
using FantasyEsportsBattle.Data;
using FantasyEsportsBattle.Host.Data.Models.Tournament;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FantasyEsportsBattle.Host.Areas.Identity.Pages.Tournaments
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _dbContext;
        public Dictionary<string,byte[]> Competitions;

        [BindProperty]
        public List<string> CheckedCompetitions { get; set; }
        [BindProperty]
        public string TournamentName { get; set; }
        [BindProperty]
        public int TournamentSize { get; set; }

        [BindProperty]
        public decimal Value { get; set; } = 2;
        public decimal ValueMin { get; set; } = 2;
        public decimal ValueMax { get; set; } = 8;

        public void OnValueChanged2(decimal val)
        {
            Value = val;
        }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

            var competitions = dbContext.Competitions.Select(c => c.Name).ToList();

            var defaultImage = _dbContext.Images.FirstOrDefault(i => i.Id == Constants.Constants.DefaultImageId)
                .ImageData;

            Competitions = new Dictionary<string, byte[]>();

            foreach (var competition in competitions)
            {

                var image = _dbContext.Images.FirstOrDefault(i => i.ImageTitle == competition)?.ImageData;
                
                Competitions.Add(competition,image == null ? defaultImage : image);
            }
        }

        public void OnGet()
        {

        }

        public ActionResult OnPostCreateTournament()
        {
            if (_dbContext.Tournaments.Any(c => c.Name == TournamentName) || Request.HttpContext?.User == null)
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

using System.Collections.Generic;
using System.Linq;
using FantasyEsportsBattle.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FantasyEsportsBattle.Host.Areas.Identity.Pages.Tournaments
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _dbContext;
        public List<string> Competitions;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            Competitions = dbContext.Competitions.Select(c => c.Name).ToList();
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {

        }
    }
}

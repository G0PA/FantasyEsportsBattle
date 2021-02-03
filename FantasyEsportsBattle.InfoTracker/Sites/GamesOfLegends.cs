using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using FantasyEsportsBattle.Data;
using FantasyEsportsBattle.Host.Data.Models;
using FantasyEsportsBattle.Host.Data.Models.Tournament;
using FantasyEsportsBattle.Host.Enumerations;
using Newtonsoft.Json.Linq;

namespace FantasyEsportsBattle.InfoTracker.Sites
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    class GamesOfLegends : Site
    {
        private readonly string _allTournamentsLink = "https://gol.gg/tournament/ajax.trlist.php";
        private readonly string _teamsUrl = "http://gol.gg/teams/";
        private readonly string _tournamentStatsLink = "https://gol.gg/tournament/tournament-stats/";
        private readonly string _iconLink = "https://gol.gg/_img/leagues_icon/";
        private Regex scriptResultsRegex = new Regex(@"data : \[([\d,]+)]", RegexOptions.Compiled | RegexOptions.Multiline);
        public override void GetLinks(ApplicationDbContext dbContext)
        {
            var values = new Dictionary<string, string>
                {
                    { "season", "S11" },
                };

            var responseString = PostRequest(_allTournamentsLink, values);

            if (responseString == null)
            {
                return;
            }

            var responseJson = JArray.Parse(responseString);

            var competitions = new List<Competition>();

            foreach (var tournament in responseJson.Where(json => dbContext.Competitions.All(competition => competition.Name != json["trname"].ToString())))
            {
                var competition = new Competition();

                competition.Region = Enum.Parse<Region>(tournament["region"].ToString());
                competition.Name = tournament["trname"].ToString();

                var uri = $"{_tournamentStatsLink}{competition.Name.Replace(" ", "%20")}/";

                var html = Client.GetStringAsync(uri).Result;

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var tournamentAlias = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'row mt-3 fond-main-cadre')]").SelectSingleNode("//span[contains(@class,'navbar-brand')]").InnerText.Split(' ')[0];

                var url = $"{_iconLink}{tournamentAlias}";

                try
                {
                    var byteArray = Client.GetByteArrayAsync(url).Result;

                    dbContext.Images.Add(new Image {ImageData = byteArray, ImageTitle = competition.Name});
                }
                catch (Exception ex)
                {
                    //NO Image found
                }

                competitions.Add(competition);
            } 

            dbContext.SaveChanges();

            foreach (var competition in competitions)
            {
                competition.ImageId = dbContext.Images.FirstOrDefault(i => i.ImageTitle == competition.Name)?.Id ?? 31;
            }

            dbContext.AddRange(competitions);
            dbContext.SaveChanges();
        }

        private string PostRequest(string url, Dictionary<string, string> values)
        {
            var content = new FormUrlEncodedContent(values);

            var response = Client.PostAsync(url, content).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

    }
}

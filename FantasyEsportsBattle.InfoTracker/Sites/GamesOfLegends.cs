using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using FantasyEsportsBattle.Host.Data.Models;
using FantasyEsportsBattle.Host.Enumerations;
using Newtonsoft.Json.Linq;

namespace FantasyEsportsBattle.InfoTracker.Sites
{
    using FantasyEsportsBattle.Web.Data;
    using FantasyEsportsBattle.Web.Data.Models.Tournament;
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
        private readonly string _leagueIconLink = "https://gol.gg/_img/leagues_icon/";
        private readonly string _teamIconLink = "https://gol.gg/_img/teams_icon/{0}-2021.png";
        private const int DefaultImageId = 1;
        private ApplicationDbContext _dbContext;
        private const string SeasonSuffix = "S11";

        private readonly string _tournamentRankingLink =
            "https://gol.gg/tournament/tournament-ranking/";
        private Regex scriptResultsRegex = new Regex(@"data : \[([\d,]+)]", RegexOptions.Compiled | RegexOptions.Multiline);
        public override void ParseWebsiteOnInterval(ApplicationDbContext dbContext,TimeSpan interval)
        {
            _dbContext = dbContext;

            while (true)
            {
                var competitions = GetCompetitions();

                if (competitions == null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    continue;
                }

                _dbContext.SaveChanges();

                foreach (var competition in competitions)
                {
                    var teams = GetTeamsForCompetition(competition);
                }

                _dbContext.SaveChanges();

                UpdateDisplayImageIds();

                _dbContext.SaveChanges();

                Thread.Sleep(interval);
            }
        }

        private void UpdateDisplayImageIds()
        {
            foreach (var competition in _dbContext.Competitions.ToList())
            {
                var imgId = FindRelevantImageId(competition.Name);

                competition.DisplayImage = imgId;
            }

            foreach (var team in _dbContext.Teams.ToList())
            {
                var imgId = FindRelevantImageId(team.Name);
                team.DisplayImage = imgId;
            }
        }

        private List<Team> GetTeamsForCompetition(Competition competition)
        {

            List<Team> teams = new List<Team>();

            try
            {
                var tournamentRankings =
                    Client.GetStringAsync(
                        $"{_tournamentRankingLink}{BuildUrlFromName(competition.Name)}/").Result;

                HtmlDocument teamsDoc = new HtmlDocument();

                teamsDoc.LoadHtml(tournamentRankings);

                var teamLinks = teamsDoc.DocumentNode.SelectSingleNode("//table").SelectNodes(".//tbody//@href")
                    .Select(n => n.Attributes["href"].Value.Trim().Replace("..", "https://gol.gg"));

                foreach (var teamLink in teamLinks)
                {
                    try
                    {
                        var teamFromDb = _dbContext.Teams.FirstOrDefault(t => t.Competition.Name == competition.Name);

                        Team team = teamFromDb == null ? new Team() : teamFromDb;

                        var doc = new HtmlDocument();

                        var content = Client.GetStringAsync(teamLink).Result;

                        doc.LoadHtml(content);

                        var teamName = doc.DocumentNode.SelectSingleNode("//h1").InnerText;

                        team.Name = teamName;

                        team.CompetitionId = competition.Id;

                        AddImageToDb(string.Format(_teamIconLink, teamName.ToLowerInvariant()), teamName);

                        var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'table_list')]");

                        foreach (var table in tables)
                        {
                            if (table.InnerText.Contains($"{teamName} - {SeasonSuffix}"))
                            {
                                var columns = table.SelectNodes(".//td");

                                for (int i = 0; i < columns.Count - 1; i += 2)
                                {
                                    if (columns[i].InnerText.Contains("Region") && columns[i].InnerText != "-")
                                    {
                                        team.Region = (Region) Enum.Parse(typeof(Region), columns[i + 1].InnerText,
                                            true);

                                        continue;
                                    }

                                    if (columns[i].InnerText.Contains("Win Rate") && columns[i].InnerText != "-")
                                    {
                                        var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "")
                                            .Split(" - ");
                                        var wins = int.Parse(winrateArr[0].Trim());
                                        var losses = int.Parse(winrateArr[1].Trim());
                                        var totalGames = wins + losses;
                                        var winrate = (int) (((double) wins / (double) totalGames) * 100);
                                        team.Winrate = winrate;
                                        team.Wins = wins;
                                        team.Losses = losses;

                                        continue;
                                    }

                                    if (columns[i].InnerText.Contains("Average game duration") &&
                                        columns[i].InnerText != "-")
                                    {
                                        var timeString = columns[i + 1].InnerText.Replace(":", ",");
                                        var time = float.Parse(timeString);
                                        team.AverageGameTime = (float) time;
                                        continue;
                                    }
                                }

                                continue;
                            }

                            if (table.InnerText.Contains("Player"))
                            {
                                var linkNodes = table.SelectNodes(".//a").Where(n => !n.InnerText.ToLowerInvariant().Contains("winrate")).ToList();

                                for (int i = 0; i < linkNodes.Count; i++)
                                {
                                    if (linkNodes[i].OuterHtml.Contains("player-stats"))
                                    {
                                        var role = table.SelectNodes(".//tr//td").Where(n =>
                                            n.Attributes.Count == 0 && !n.InnerText.Contains("&nbsp") &&
                                            !n.InnerText.ToLowerInvariant().Contains("winrate")).ToList()[i].InnerText;

                                        var uriString = linkNodes[i].Attributes["href"].Value;
                                        uriString = uriString.Substring(2);
                                        var uri = new Uri("http://gol.gg" + uriString);

                                        UpdatePlayerForTeam(team, uri);
                                    }
                                }
                            }
                        }

                        if (teamFromDb == null)
                        {
                            _dbContext.Teams.Add(team);
                        }

                        teams.Add(team);

                    }
                    catch (Exception ex)
                    {
                        //error parsing team
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return teams;
        }

        private void UpdatePlayerForTeam(Team team, Uri uri)
        {
            var responseString = Client.GetStringAsync(uri).Result;

            if (responseString == null)
            {
                return;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);
            var playerName = doc.DocumentNode.SelectSingleNode("//h1[contains(@class,'panel-title')]")?.InnerText;

            var exists = _dbContext.TournamentPlayers.Where(t => t.Team.Name == team.Name)
                .Any(t => t.Nickname == playerName);

            if (playerName == null)
            {
                return;
            }

            var player = exists
                ? _dbContext.TournamentPlayers.FirstOrDefault(t => t.Team.Name == playerName)
                : new TournamentPlayer();

            player.TeamId = team.Id;

            if (!exists)
            {
                _dbContext.TournamentPlayers.Add(player);
            }
        }

        private List<Competition> GetCompetitions()
        {
            var values = new Dictionary<string, string>
            {
                {"season", "S11"},
            };

            var responseString = PostRequest(_allTournamentsLink, values);

            if (responseString == null)
            {
                return null;
            }

            var responseJson = JArray.Parse(responseString);

            var competitions = new List<Competition>();

            foreach (var tournament in responseJson)
            {
                var competitionName = tournament["trname"].ToString();

                var exists = _dbContext.Competitions.Any(c => c.Name == competitionName);

                var competition = exists ? _dbContext.Competitions.FirstOrDefault(c => c.Name == competitionName) : new Competition();

                if(Enum.TryParse(typeof(Region),tournament["region"].ToString(),out var region))
                {
                    competition.Region = Enum.Parse<Region>(tournament["region"].ToString());
                }
                else
                {
                    Console.WriteLine($"Unable to find Region Enum for {tournament["region"].ToString()}");
                }

                competition.Name = tournament["trname"].ToString();

                var uri = $"{_tournamentStatsLink}{BuildUrlFromName(competition.Name)}/";

                var html = Client.GetStringAsync(uri).Result;

                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(html);

                var tournamentAlias = doc.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'row mt-3 fond-main-cadre')]")
                    .SelectSingleNode("//span[contains(@class,'navbar-brand')]").InnerText.Split(' ')[0];

                var url = $"{_leagueIconLink}{tournamentAlias}";

                AddImageToDb(url, competition.Name);

                competitions.Add(competition);

                if (!exists)
                {
                    _dbContext.Competitions.Add(competition);
                }
            }

            return competitions;
        }

        private int FindRelevantImageId(string imageTitle)
        {
            return _dbContext.Images.FirstOrDefault(i => i.ImageTitle == imageTitle)?.Id ?? DefaultImageId;
        }

        private string BuildUrlFromName(string name)
        {
            return name.Replace(" ", "%20");
        }

        private bool AddImageToDb(string url, string imageTitle)
        {
            try
            {
                var byteArray = Client.GetByteArrayAsync(url).Result;

                var image = _dbContext.Images.FirstOrDefault(i => i.ImageTitle == imageTitle);

                if (image != null)
                {
                    image.ImageData = byteArray;
                }
                else
                {
                    _dbContext.Images.Add(new Image {ImageData = byteArray, ImageTitle = imageTitle});
                }

                return true;
            }
            catch (Exception ex)
            {
                //NO Image found
            }

            return false;
        }

        private string PostRequest(string url, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                var response = Client.PostAsync(url, content).Result;

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

    }
}

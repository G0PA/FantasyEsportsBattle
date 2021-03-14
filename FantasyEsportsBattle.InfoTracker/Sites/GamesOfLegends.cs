using System.Net.Http;
using System.Threading;
using FantasyEsportsBattle.Host.Data.Models;
using FantasyEsportsBattle.Web.Enumerations;
using Newtonsoft.Json.Linq;
using Serilog;

namespace FantasyEsportsBattle.InfoTracker.Sites
{
    using FantasyEsportsBattle.Web.Data;
    using FantasyEsportsBattle.Web.Data.Models.Tournament;
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private readonly string _tournamentMatchListLink =
            "https://gol.gg/tournament/tournament-matchlist/";
        private Regex scriptResultsRegex = new Regex(@"data : \[([\d,]+)]", RegexOptions.Compiled | RegexOptions.Multiline);
        public override void ParseWebsiteOnInterval(ApplicationDbContext dbContext, TimeSpan interval)
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

                    GetFinishedMatchesForCompetition(competition, teams);
                }

                _dbContext.SaveChanges();

                UpdateDisplayImageIds();

                _dbContext.SaveChanges();

                Console.WriteLine("Parsing Done");

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
            Console.WriteLine($"Parsing competition {competition.Name}");
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
                        var doc = new HtmlDocument();

                        var content = Client.GetStringAsync(teamLink).Result;

                        doc.LoadHtml(content);

                        var teamName = doc.DocumentNode.SelectSingleNode("//h1").InnerText;

                        var teamFromDb = _dbContext.Teams.FirstOrDefault(t => t.Name == teamName);

                        Team team = teamFromDb == null ? new Team() : teamFromDb;

                        team.Name = teamName;

                        Console.WriteLine($"Parsing team {team.Name}");

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
                                        team.Region = (Region)Enum.Parse(typeof(Region), columns[i + 1].InnerText,
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
                                        var winrate = (int)(((double)wins / (double)totalGames) * 100);
                                        team.Winrate = winrate;
                                        team.Wins = wins;
                                        team.Losses = losses;
                                        team.TotalGames = totalGames;
                                        continue;
                                    }

                                    if (columns[i].InnerText.Contains("Average game duration") &&
                                        columns[i].InnerText != "-")
                                    {
                                        var timeString = columns[i + 1].InnerText.Replace(":", ",");
                                        var time = float.Parse(timeString);
                                        team.AverageGameTime = (float)time;
                                        continue;
                                    }
                                }
                            }

                            if (table.InnerText.Contains("Player"))
                            {
                                var linkNodes = table.SelectNodes(".//a").Where(n => !n.InnerText.ToLowerInvariant().Contains("winrate")).ToList();

                                for (int i = 0; i < linkNodes.Count; i++)
                                {
                                    if (linkNodes[i].OuterHtml.Contains("player-stats"))
                                    {
                                        var role = table.SelectNodes(".//tr//td").Where(n =>
                                            n.Attributes.Count == 0
                                            && !n.InnerText.Contains("&nbsp")
                                            && !n.InnerHtml.Contains("stats")
                                            && !n.InnerText.ToLowerInvariant().Contains("winrate")).ToList()[i].InnerText;

                                        var uriString = linkNodes[i].Attributes["href"].Value;
                                        uriString = uriString.Substring(2);
                                        var uri = new Uri("http://gol.gg/" + uriString);

                                        UpdatePlayerForTeam(team, uri, role);
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
                        Log.Logger.Error($"Error parsing team for teamLink {teamLink} {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error parsing competition {competition.Name} {ex.Message}");
            }

            return teams;
        }

        private void GetFinishedMatchesForCompetition(Competition competition, List<Team> teams)
        {
            var tournamentMatches =
                Client.GetStringAsync(
                    $"{_tournamentMatchListLink}{BuildUrlFromName(competition.Name)}/").Result;

            var doc = new HtmlDocument();

            doc.LoadHtml(tournamentMatches);

            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'table_list')]");

            var columns = table.SelectNodes(".//tr").Where(x => !x.InnerHtml.Contains("> - <")).Skip(1).ToList();

            for (int i = 0; i < columns.Count; i++)
            {
                var currentGame = columns[i].SelectNodes(".//td");

                var blueSideTeam = currentGame[1].InnerHtml;
                var redSideTeam = currentGame[3].InnerHtml;

                var score = currentGame[2].InnerHtml.Split("-");
                var blueSideTeamScore = int.Parse(score[0].Trim());
                var redSideTeamScore = int.Parse(score[1].Trim());
                var date = DateTime.Parse(currentGame[6].InnerHtml);

                var finishedEventFromDb = _dbContext
                    .FinishedEvents
                    .FirstOrDefault(t => t.HomeTeam.Name == blueSideTeam
                                    && t.AwayTeam.Name == redSideTeam
                                    && t.GameDate == date);

                if (finishedEventFromDb == null)
                {
                    var homeTeam = teams.FirstOrDefault(t => t.Name == blueSideTeam);
                    var awayTeam = teams.FirstOrDefault(t => t.Name == redSideTeam);

                    var finishedEvent = new FinishedEvent
                    {
                        HomeTeam = homeTeam,
                        AwayTeam = awayTeam,
                        Competition = competition,
                        GameDate = date,
                        Score1 = blueSideTeamScore,
                        Score2 = redSideTeamScore,
                    };

                    _dbContext.FinishedEvents.Add(finishedEvent);
                }
            }
        }

        private void UpdatePlayerForTeam(Team team, Uri uri, string role)
        {
            try
            {
                var responseString = Client.GetStringAsync(uri).Result;

                if (responseString == null)
                {
                    return;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(responseString);
                var playerName = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText.Replace("&nbsp; ", "").Trim();

                var exists = _dbContext.CompetitionPlayers.Where(t => t.Team.Name == team.Name)
                    .Any(t => t.Nickname == playerName);

                if (playerName == null)
                {
                    return;
                }

                var player = exists
                    ? _dbContext.CompetitionPlayers.FirstOrDefault(t => t.Nickname == playerName)
                    : new CompetitionPlayer();

                player.Role = (Roles)Enum.Parse(typeof(Roles), role, true);
                player.Team = team;
                player.Nickname = playerName;

                Console.WriteLine($"Parsing player {player.Nickname} for team {player.Team.Name} in role {player.Role}");

                PopulatePlayerStats(doc,player);

                if (!exists)
                {
                    _dbContext.CompetitionPlayers.Add(player);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error updating player for team {team.Name} {ex.Message}");
            }
        }

        private void PopulatePlayerStats(HtmlDocument doc, CompetitionPlayer player)
        {
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class,'table_list')]");

            if (tables == null)
            {
                return;
            }

            List<Task> tasks = new List<Task>();

            foreach (var table in tables)
            {
                try
                {
                    if (table.InnerText.Contains("General stats"))
                    {
                        var columns = table.SelectNodes(".//td");
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Record") && columns[i + 1].InnerText != "-")
                                {
                                    var winrateArr = columns[i + 1].InnerText.Replace("W", "").Replace("L", "").Split(" - ");
                                    var wins = int.Parse(winrateArr[0].Trim());
                                    var losses = int.Parse(winrateArr[1].Trim());
                                    player.Wins = wins;
                                    player.Losses = losses;
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("KDA") && columns[i + 1].InnerText != "-")
                                {
                                    try
                                    {
                                        player.KDA = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    }
                                    catch
                                    {

                                    }
                                }
                                if (columns[i].InnerText.Contains("CS per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    player.CSPM = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    player.GPM = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold%") && columns[i + 1].InnerText != "-")
                                {
                                    player.GoldPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Kill Participation") && columns[i + 1].InnerText != "-")
                                {
                                    player.KillParticipationPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                    continue;
                                }
                            }
                        }));
                    }
                    if (table.InnerText.Contains("Early game"))
                    {
                        var columns = table.SelectNodes(".//td");
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Ahead in CS at 15 min") && columns[i + 1].InnerText != "-")
                                {
                                    var percentString = columns[i + 1].InnerText.Replace("%", "").Replace(".", ",").Trim().Replace("&nbsp;", "").Trim();
                                    player.AheadInCSAt15MinPercent = float.Parse(percentString);
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("CS Differential at 15 min") && columns[i + 1].InnerText != "-")
                                {
                                    player.CSDifferenceAt15Min = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Gold Differential at 15 min") && columns[i + 1].InnerText != "-")
                                {
                                    player.GoldDifferenceAt15Min = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("XP Differential at 15 min") && columns[i + 1].InnerText != "-")
                                {
                                    player.XPDifferenceAt15Min = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Blood Participation") && columns[i + 1].InnerText != "-")
                                {
                                    player.FirstBloodParticipationPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("First Blood Victim") && columns[i + 1].InnerText != "-")
                                {
                                    player.FirstBloodVictimPercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                    continue;
                                }
                            }
                        }));
                    }
                    if (table.InnerText.Contains("Aggression"))
                    {
                        var columns = table.SelectNodes(".//td");
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Damage Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    player.DamagePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                                if (columns[i].InnerText.Contains("Damage%") && columns[i + 1].InnerText != "-")
                                {
                                    player.DamagePercent = float.Parse(columns[i + 1].InnerText.Replace(".", ",").Replace("%", "").Trim());
                                    continue;
                                }
                            }
                        }));
                    }
                    if (table.InnerText.Contains("Vision"))
                    {
                        var columns = table.SelectNodes(".//td");
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            for (int i = 0; i < columns.Count - 1; i += 2)
                            {
                                if (columns[i].InnerText.Contains("Vision score Per Minute") && columns[i + 1].InnerText != "-")
                                {
                                    player.VisionScorePerMinute = float.Parse(columns[i + 1].InnerText.Replace(".", ","));
                                    continue;
                                }
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Error($"Error parsing Player column! PlayerName: {player.Nickname}", ex);
                }
            }

            Task.WaitAll(tasks.ToArray());
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

                try
                {
                    competition.Region = Enum.Parse<Region>(tournament["region"].ToString());
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"New Region! Add '{tournament["region"]}' {e.Message}");
                    continue;
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
                    _dbContext.Images.Add(new Image { ImageData = byteArray, ImageTitle = imageTitle });
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error adding image {url} {ex.Message}");
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
                Log.Logger.Error($"Error creating request to url {url} {ex.Message}");
            }

            return null;
        }

    }
}

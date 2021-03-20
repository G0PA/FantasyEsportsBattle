using FantasyEsportsBattle.Web.Data.Models.Tournament;
using FantasyEsportsBattle.Web.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyEsportsBattle.Web.Extensions
{
    public static class TournamentExtensions
    {
        public static Dictionary<CompetitionPlayer,float> CalculatePointsForAllPlayers(this Tournament tournament)
        {
            var playersWithGrades = new Dictionary<Role, Dictionary<CompetitionPlayer, float>>();

            playersWithGrades.Add(Role.TOP, GetAllPlayersInTournamentForRole(tournament, Role.TOP));
            playersWithGrades.Add(Role.JUNGLE, GetAllPlayersInTournamentForRole(tournament, Role.JUNGLE));
            playersWithGrades.Add(Role.MID, GetAllPlayersInTournamentForRole(tournament, Role.MID));
            playersWithGrades.Add(Role.ADC, GetAllPlayersInTournamentForRole(tournament, Role.ADC));
            playersWithGrades.Add(Role.SUPPORT, GetAllPlayersInTournamentForRole(tournament, Role.SUPPORT));

            var orderedPlayersWithGrades = new Dictionary<CompetitionPlayer, float>();

            switch (tournament.TournamentAlgorithm)
            {
                case TournamentAlgorithm.Standard:
                    {
                        GradePlayers(playersWithGrades[Role.TOP]);
                        GradePlayers(playersWithGrades[Role.JUNGLE]);
                        GradePlayers(playersWithGrades[Role.MID]);
                        GradePlayers(playersWithGrades[Role.ADC]);
                        GradePlayers(playersWithGrades[Role.SUPPORT]);

                        Dictionary<CompetitionPlayer, float> allPlayers = new Dictionary<CompetitionPlayer, float>();

                        foreach (var rolePlayers in playersWithGrades.Values)
                        {
                            rolePlayers.ToList().ForEach(rp => allPlayers.Add(rp.Key, rp.Value));
                        }

                        foreach (var player in allPlayers.OrderByDescending(rp => rp.Value))
                        {
                            orderedPlayersWithGrades.Add(player.Key, player.Value);
                        }

                        break;
                    }
            }

            return orderedPlayersWithGrades;
        }

        private static void GradePlayers(Dictionary<CompetitionPlayer,float> players)
        {
            var playersWithNoGames = players.Where(p => p.Key.Wins == 0 && p.Key.Losses == 0).ToList();
            var playersWithGames = players.Where(p => playersWithNoGames.All(pwng => pwng.Key != p.Key)).ToList();

            players.Clear();

            foreach(var player in playersWithGames)
            {
                players.Add(player.Key, player.Value);
            }
            var orderedPlayersByWinrate = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByWinrate = orderedPlayersByWinrate.OrderByDescending(p => p.Winrate).ToList();

            for(int i=0; i<orderedPlayersByWinrate.Count; i++)
            {
                players[orderedPlayersByWinrate[i]] += GivePoints(GetPositionWinrate(orderedPlayersByWinrate,i), orderedPlayersByWinrate.Count, MaxGrade.Winrate);
            }

            var orderedPlayersByKDA = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByKDA = orderedPlayersByKDA.OrderByDescending(p => p.KDA).ToList();

            for (int i = 0; i < orderedPlayersByKDA.Count; i++)
            {
                players[orderedPlayersByKDA[i]] += GivePoints(GetPositionKDA(orderedPlayersByKDA, i), orderedPlayersByKDA.Count, MaxGrade.KDA);
            }

            var orderedPlayersByCSPM = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByCSPM = orderedPlayersByCSPM.OrderByDescending(p => p.CSPM).ToList();

            for (int i = 0; i < orderedPlayersByCSPM.Count; i++)
            {
                players[orderedPlayersByCSPM[i]] += GivePoints(GetPositionCSPM(orderedPlayersByCSPM, i), orderedPlayersByCSPM.Count, MaxGrade.CSPM);
            }

            var orderedPlayersByGPM = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByGPM = orderedPlayersByGPM.OrderByDescending(p => p.GPM).ToList();

            for (int i = 0; i < orderedPlayersByGPM.Count; i++)
            {
                players[orderedPlayersByGPM[i]] += GivePoints(GetPositionGPM(orderedPlayersByCSPM, i), orderedPlayersByGPM.Count, MaxGrade.GPM);
            }

            var orderedPlayersByKillParticipationPercent = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByKillParticipationPercent = orderedPlayersByKillParticipationPercent.OrderByDescending(p => p.KillParticipationPercent).ToList();

            for (int i = 0; i < orderedPlayersByKillParticipationPercent.Count; i++)
            {
                players[orderedPlayersByKillParticipationPercent[i]] += GivePoints(GetPositionKillParticipationPercent(orderedPlayersByKillParticipationPercent, i), orderedPlayersByKillParticipationPercent.Count, MaxGrade.KillParticipationPercent);
            }

            var orderedPlayersByAheadInCSAt15MinPercent = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByAheadInCSAt15MinPercent = orderedPlayersByAheadInCSAt15MinPercent.OrderByDescending(p => p.AheadInCSAt15MinPercent).ToList();

            for (int i = 0; i < orderedPlayersByAheadInCSAt15MinPercent.Count; i++)
            {
                players[orderedPlayersByAheadInCSAt15MinPercent[i]] += GivePoints(GetPositionAheadInCSAt15MinPercent(orderedPlayersByAheadInCSAt15MinPercent, i), orderedPlayersByAheadInCSAt15MinPercent.Count, MaxGrade.AheadInCSAt15MinPercent);
            }

            var orderedPlayersByCSDifferenceAt15 = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByCSDifferenceAt15 = orderedPlayersByCSDifferenceAt15.OrderByDescending(p => p.CSDifferenceAt15Min).ToList();

            for (int i = 0; i < orderedPlayersByCSDifferenceAt15.Count; i++)
            {
                players[orderedPlayersByCSDifferenceAt15[i]] += GivePoints(GetPositionCSDifferenceAt15(orderedPlayersByCSDifferenceAt15, i), orderedPlayersByCSDifferenceAt15.Count, MaxGrade.CSDifferenceAt15Min);
            }

            var orderedPlayersByGoldDifferenceAt15 = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByGoldDifferenceAt15 = orderedPlayersByGoldDifferenceAt15.OrderByDescending(p => p.GoldDifferenceAt15Min).ToList();

            for (int i = 0; i < orderedPlayersByGoldDifferenceAt15.Count; i++)
            {
                players[orderedPlayersByGoldDifferenceAt15[i]] += GivePoints(GetPositionGoldDifferenceAt15(orderedPlayersByGoldDifferenceAt15, i), orderedPlayersByGoldDifferenceAt15.Count, MaxGrade.GoldDifferenceAt15Min);
            }

            var orderedPlayersByXPDifferenceAt15 = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByXPDifferenceAt15 = orderedPlayersByXPDifferenceAt15.OrderByDescending(p => p.XPDifferenceAt15Min).ToList();

            for (int i = 0; i < orderedPlayersByXPDifferenceAt15.Count; i++)
            {
                players[orderedPlayersByXPDifferenceAt15[i]] += GivePoints(GetPositionXPDifferenceAt15(orderedPlayersByXPDifferenceAt15, i), orderedPlayersByXPDifferenceAt15.Count, MaxGrade.XPDifferenceAt15Min);
            }

            var orderedPlayersByDamagePercent = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByDamagePercent = orderedPlayersByDamagePercent.OrderByDescending(p => p.DamagePercent).ToList();

            for (int i = 0; i < orderedPlayersByDamagePercent.Count; i++)
            {
                players[orderedPlayersByDamagePercent[i]] += GivePoints(GetPositionDamagePercent(orderedPlayersByDamagePercent, i), orderedPlayersByDamagePercent.Count, MaxGrade.DamagePercent);
            }

            var orderedPlayersByVisionScorePerMinute = new List<CompetitionPlayer>(players.Keys);

            orderedPlayersByVisionScorePerMinute = orderedPlayersByVisionScorePerMinute.OrderByDescending(p => p.VisionScorePerMinute).ToList();

            for (int i = 0; i < orderedPlayersByVisionScorePerMinute.Count; i++)
            {
                players[orderedPlayersByVisionScorePerMinute[i]] += GivePoints(GetPositionVisionScorePerMinute(orderedPlayersByVisionScorePerMinute, i), orderedPlayersByVisionScorePerMinute.Count, MaxGrade.VisionScorePerMinute);
            }

            foreach(var player in playersWithNoGames)
            {
                players.Add(player.Key, 0);
            }
        }

        private static int GetPositionWinrate(List<CompetitionPlayer> orderedPlayers,int position)
        {
            while (position > 0 && orderedPlayers[position].Winrate == orderedPlayers[position - 1].Winrate)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionKDA(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].KDA == orderedPlayers[position - 1].KDA)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionCSPM(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].CSPM == orderedPlayers[position - 1].CSPM)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionGPM(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].GPM == orderedPlayers[position - 1].GPM)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionKillParticipationPercent(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].KillParticipationPercent == orderedPlayers[position - 1].KillParticipationPercent)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionAheadInCSAt15MinPercent(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].AheadInCSAt15MinPercent == orderedPlayers[position - 1].AheadInCSAt15MinPercent)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionCSDifferenceAt15(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].CSDifferenceAt15Min == orderedPlayers[position - 1].CSDifferenceAt15Min)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionGoldDifferenceAt15(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].GoldDifferenceAt15Min == orderedPlayers[position - 1].GoldDifferenceAt15Min)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionXPDifferenceAt15(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].XPDifferenceAt15Min == orderedPlayers[position - 1].XPDifferenceAt15Min)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionDamagePercent(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].DamagePercent == orderedPlayers[position - 1].DamagePercent)
            {
                position--;
            }

            return position;
        }

        private static int GetPositionVisionScorePerMinute(List<CompetitionPlayer> orderedPlayers, int position)
        {
            while (position > 0 && orderedPlayers[position].VisionScorePerMinute == orderedPlayers[position - 1].VisionScorePerMinute)
            {
                position--;
            }

            return position;
        }

        private static float GivePoints(int positionInTournament, int totalParticipants,MaxGrade maxGrade)
        {
            var maxGradeValue = (int) maxGrade;

            float pointsDeductionPerPosition = (float) maxGradeValue / (float)totalParticipants;

            return maxGradeValue - (positionInTournament * pointsDeductionPerPosition);
        }

        private static Dictionary<CompetitionPlayer,float> GetAllPlayersInTournamentForRole(Tournament tournament, Role role)
        {
            var allTeams = tournament.TournamentCompetitions.Select(tc => tc.Competition).Select(c => c.Teams);

            Dictionary<CompetitionPlayer,float> players = new Dictionary<CompetitionPlayer,float>();

            foreach (var teamCollection in allTeams)
            {
                foreach (var team in teamCollection)
                {
                    foreach (var player in team.Players.Where(p => p.Role == role))
                    {
                        players.Add(player,0);
                    }
                }
            }

            return players;
        }
        public static int GetMaxPoints()
        {
            return Enum.GetValues<MaxGrade>().Select(m => (int) m).ToList().Sum();
        }

        private enum MaxGrade
        {
            Winrate = 10,
            KDA = 10,
            CSPM = 5,
            GPM = 5,
            KillParticipationPercent = 10,
            AheadInCSAt15MinPercent = 10,
            CSDifferenceAt15Min = 10,
            GoldDifferenceAt15Min = 10,
            XPDifferenceAt15Min = 10,
            DamagePercent = 10,
            VisionScorePerMinute = 10,
        }
    }
}

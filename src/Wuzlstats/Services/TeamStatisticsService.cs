using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Teams;

namespace Wuzlstats.Services
{
    public class TeamStatisticsService
    {
        private readonly Db _db;

        public TeamStatisticsService(Db db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TeamViewModel>> FindTeamsOfLeagueAsync(int leagueId, int? daysForStatistics)
        {
            var gamesQuery = FetchGames(leagueId, daysForStatistics);

            var teams = new List<TeamViewModel>();

            var players2 = await _db.Players.Where(x => x.LeagueId == leagueId).ToListAsync();

            var players = players2.Select(player => new PlayerViewModel
            {
                Id = player.Id,
                Name = player.Name,
                Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image)
            }).ToList();

            foreach (var game in await gamesQuery.ToListAsync())
            {
                var positions = await (from position in _db.PlayerPositions.AsNoTracking()
                                       join player in _db.Players.AsNoTracking() on position.PlayerId equals player.Id
                                       where position.GameId == game.Id
                                       where position.Position != PlayerPositionTypes.Blue || position.Position != PlayerPositionTypes.Red
                                       select new
                                       {
                                           position.Position,
                                           Player = player
                                       }).ToListAsync();

                // team stats
                if (positions.Count(x => x.Position == PlayerPositionTypes.BlueDefense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.BlueOffense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.RedDefense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.RedOffense) == 1)
                {
                    var redOffense = players.Single(x => x.Id == positions.Single(y => y.Position == PlayerPositionTypes.RedOffense).Player.Id);
                    var redDefense = players.Single(x => x.Id == positions.Single(y => y.Position == PlayerPositionTypes.RedDefense).Player.Id);
                    var blueOffense = players.Single(x => x.Id == positions.Single(y => y.Position == PlayerPositionTypes.BlueOffense).Player.Id);
                    var blueDefense = players.Single(x => x.Id == positions.Single(y => y.Position == PlayerPositionTypes.BlueDefense).Player.Id);

                    var redTeam = teams.FirstOrDefault(x => x.Equals(redOffense, redDefense));
                    if (redTeam == null)
                    {
                        redTeam = CreateTeam(redOffense, redDefense);
                        teams.Add(redTeam);
                    }
                    var blueTeam = teams.FirstOrDefault(x => x.Equals(blueOffense, blueDefense));
                    if (blueTeam == null)
                    {
                        blueTeam = CreateTeam(blueOffense, blueDefense);
                        teams.Add(blueTeam);
                    }

                    if (game.BlueWins)
                    {
                        redTeam.Losses++;
                        blueTeam.Wins++;
                    }
                    else if (game.RedWins)
                    {
                        redTeam.Wins++;
                        blueTeam.Losses++;
                    }
                    //Resolve date of last played game
                    ResolveLastGamePlayedOn(redTeam, game.Date);
                    ResolveLastGamePlayedOn(blueTeam, game.Date);
                }
            }

            return teams;
        }


        private void ResolveLastGamePlayedOn(TeamViewModel team, DateTime date)
        {
            if (team.LastGamePlayedOn < date)
            {
                team.LastGamePlayedOn = date;
            }
        }

        public static TeamViewModel CreateTeam(PlayerViewModel p1, PlayerViewModel p2)
        {
            if (p1.Id <= p2.Id)
            {
                return new TeamViewModel
                {
                    Player1 = p1,
                    Player2 = p2
                };
            }
            return new TeamViewModel
            {
                Player1 = p2,
                Player2 = p1
            };
        }

        private IQueryable<Game> FetchGames(int leagueId, int? daysForStatistics)
        {
            var gamesQuery = _db.Games.AsNoTracking().Where(x => x.LeagueId == leagueId);
            if (!daysForStatistics.HasValue)
            {
                return gamesQuery;
            }
            var date = DateTime.UtcNow.Date.AddDays(-daysForStatistics.Value);
            return gamesQuery.Where(x => x.Date >= date);
        }
    }
}

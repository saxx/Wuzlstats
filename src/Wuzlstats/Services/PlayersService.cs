using Microsoft.EntityFrameworkCore;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Players;

namespace Wuzlstats.Services
{
    public class PlayersService
    {
        private readonly Db _db;

        public PlayersService(Db db)
        {
            _db = db;
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

        public async Task<IEnumerable<PlayerViewModel>> FindPlayersOfLeague(int leagueId, int? daysForStatistics)
        {
            var gamesQuery = FetchGames(leagueId, daysForStatistics);

            // EF7 beta4 does not support navigation properties in queries yet
            // this complicates the code a lot, because we need joins :(

            var players = new List<PlayerViewModel>();

            foreach (var game in await gamesQuery.ToListAsync())
            {
                var positions = await (from position in _db.PlayerPositions.AsNoTracking()
                                       join player in _db.Players.AsNoTracking() on position.PlayerId equals player.Id
                                       where position.GameId == game.Id
                                       select new
                                       {
                                           position.Position,
                                           Player = player
                                       }).ToListAsync();
                // player stats
                foreach (var position in positions)
                {
                    var playerEntity = position.Player;

                    var player = players.FirstOrDefault(x => x.PlayerId == playerEntity.Id);
                    if (player == null)
                    {
                        player = new PlayerViewModel
                        {
                            PlayerId = playerEntity.Id,
                            Name = playerEntity.Name,
                            Image = playerEntity.Image == null || playerEntity.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(playerEntity.Image)
                        };
                        players.Add(player);
                    }

                    // calculate count of single or team games
                    if (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.Red)
                    {
                        player.SingleGames++;
                    }
                    else
                    {
                        player.TeamGames++;
                    }

                    // don't count ties
                    if (game.BlueWins && (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense))
                    {
                        player.Wins++;
                    }
                    else if (game.RedWins && (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense))
                    {
                        player.Wins++;
                    }
                    else if (game.RedWins &&
                             (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense))
                    {
                        player.Losses++;
                    }
                    else if (game.BlueWins &&
                             (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense))
                    {
                        player.Losses++;
                    }
                    if (game.Date > player.LastGamePlayedOn)
                    {
                        player.LastGamePlayedOn = game.Date;
                    }
                }
            }
            return players.OrderByDescending(x => x.LastGamePlayedOn);
        }

    }
}

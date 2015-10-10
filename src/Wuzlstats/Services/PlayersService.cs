using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.Services
{
    public class PlayersService
    {
        private readonly Db _db;

        public PlayersService(Db db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PlayerDto>> FindPlayersOfLeague(int leagueId, int? daysForStatistics)
        {
            var gamesQuery = _db.Games.AsNoTracking().Where(x => x.LeagueId == leagueId);
            if (daysForStatistics.HasValue)
            {
                var date = DateTime.UtcNow.Date.AddDays(-daysForStatistics.Value);
                gamesQuery = gamesQuery.Where(x => x.Date >= date);
            }

            // EF7 beta4 does not support navigation properties in queries yet
            // this complicates the code a lot, because we need joins :(

            var players = new List<PlayerDto>();

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
                    var player = players.FirstOrDefault(x => x.Equals(position.Player));
                    if (player == null)
                    {
                        player = PlayerDto.Create(position.Player);
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
                    if (game.Date > player.LatestGame)
                    {
                        player.LatestGame = game.Date;
                    }
                }
            }
            return players.OrderByDescending(x => x.LatestGame);
        }
    }

    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int SingleGames { get; set; }
        public int TeamGames { get; set; }
        public DateTime LatestGame { get; set; }

        public double Rank => Losses == 0 ? Wins : (Wins == 0 ? 0.1d / Losses : (double)Wins / Losses);

        public bool Equals(Player p)
        {
            return Id == p.Id;
        }

        public static PlayerDto Create(Player p)
        {
            return new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image
            };
        }
    }
}

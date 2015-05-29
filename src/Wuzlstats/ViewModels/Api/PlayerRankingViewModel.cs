using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Api
{
    public class PlayerRankingViewModel
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public PlayerRankingViewModel(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<PlayerRankingViewModel> Fill(League league, int count)
        {
            var date = DateTime.UtcNow.Date.AddDays(-_settings.PlayerRankingDays);
            var gamesQuery = _db.Games.Where(x => x.LeagueId == league.Id && x.Date >= date);

            // EF7 beta4 does not support navigation properties in queries yet
            // this complicates the code a lot, because we need joins :(

            var wins = await (from p in _db.PlayerPositions
                              join game in gamesQuery on p.GameId equals game.Id
                              where game.BlueScore > game.RedScore &&
                              (p.Position == PlayerPositionTypes.Blue || p.Position == PlayerPositionTypes.BlueDefense || p.Position == PlayerPositionTypes.BlueOffense)
                              select p.PlayerId).ToListAsync();
            var losses = await (from p in _db.PlayerPositions
                                join game in gamesQuery on p.GameId equals game.Id
                                where game.BlueScore < game.RedScore &&
                                (p.Position == PlayerPositionTypes.Blue || p.Position == PlayerPositionTypes.BlueDefense || p.Position == PlayerPositionTypes.BlueOffense)
                                select p.PlayerId).ToListAsync();
            wins.AddRange(from p in _db.PlayerPositions
                          join game in gamesQuery on p.GameId equals game.Id
                          where game.BlueScore < game.RedScore &&
                          (p.Position == PlayerPositionTypes.Red || p.Position == PlayerPositionTypes.RedDefense || p.Position == PlayerPositionTypes.RedOffense)
                          select p.PlayerId);
            losses.AddRange(from p in _db.PlayerPositions
                            join game in gamesQuery on p.GameId equals game.Id
                            where game.BlueScore > game.RedScore &&
                            (p.Position == PlayerPositionTypes.Red || p.Position == PlayerPositionTypes.RedDefense || p.Position == PlayerPositionTypes.RedOffense)
                            select p.PlayerId);


            var result = new List<Player>();

            foreach (var playerId in wins)
            {
                var playerResult = result.FirstOrDefault(x => x.id == playerId);
                if (playerResult == null)
                {
                    var player = await _db.Players.SingleAsync(x => x.Id == playerId);
                    playerResult = new Player
                    {
                        name = player.Name,
                        id = player.Id,
                        image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image)
                    };
                    result.Add(playerResult);
                }
                playerResult.wins++;
            }

            foreach (var playerId in losses)
            {
                var playerResult = result.FirstOrDefault(x => x.id == playerId);
                if (playerResult == null)
                {
                    var player = await _db.Players.SingleAsync(x => x.Id == playerId);
                    playerResult = new Player
                    {
                        name = player.Name,
                        id = player.Id,
                        image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image)
                    };
                    result.Add(playerResult);
                }
                playerResult.losses++;
            }

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (count < 0)
            {
                players = result.OrderBy(x => x.rank).Take(-count).ToList();
            }
            else
            {
                players = result.OrderByDescending(x => x.rank).Take(count).ToList();
            }

            return this;
        }

        // ReSharper disable InconsistentNaming
        public IEnumerable<Player> players { get; set; }

        public class Player
        {
            public int id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }

            // ReSharper disable once PossibleLossOfFraction
            public double rank => losses == 0 ? wins : (wins == 0 ? 0.1 / losses : wins / losses);
        }
        // ReSharper restore InconsistentNaming
    }
}
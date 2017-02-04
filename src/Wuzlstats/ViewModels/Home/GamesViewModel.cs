using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Wuzlstats.ViewModels.Home
{
    public class GamesViewModel
    {
        private readonly Db _db;
        private readonly ILogger _logger;

        public GamesViewModel(Db db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger(typeof(GamesViewModel));
        }


        public async Task<GamesViewModel> Fill(League league)
        {
            _logger.LogTrace("Building GamesViewModel ...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            League = league.Name;
            Games = await _db.Games.AsNoTracking().Where(x => x.LeagueId == league.Id).OrderByDescending(x => x.Date).Select(x => new Game
            {
                Id = x.Id,
                Date = x.Date,
                RedScore = x.RedScore,
                BlueScore = x.BlueScore
            }).ToListAsync();
            _logger.LogTrace($"Loading {Games.Count} games took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var allPlayers = await _db.Players.AsNoTracking().Where(x => x.LeagueId == league.Id).Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();
            _logger.LogTrace($"Loading {allPlayers.Count} players took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var allGameIds = Games.Select(x => x.Id).ToList();
            var allPositions = await _db.PlayerPositions.AsNoTracking().Where(x => allGameIds.Contains(x.GameId)).ToListAsync();
            _logger.LogTrace($"Loading {allPositions.Count} player positions took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            foreach (var game in Games)
            {
                var positions = allPositions.Where(x => x.GameId == game.Id).ToList();
                game.RedPlayers = positions.Where(x => x.IsRedPosition)
                    .Select(x => allPlayers.Single(y => y.Id == x.PlayerId).Name)
                    .Aggregate("", (seed, value) => seed + ", " + value)
                    .Trim(',', ' ');
                game.BluePlayers = positions.Where(x => x.IsBluePosition)
                    .Select(x => allPlayers.Single(y => y.Id == x.PlayerId).Name)
                    .Aggregate("", (seed, value) => seed + ", " + value)
                    .Trim(',', ' ');
            }
            _logger.LogTrace($"Filling missing games information took {stopwatch.ElapsedMilliseconds}ms.");

            return this;
        }


        public string League { get; set; }
        public IList<Game> Games { get; set; }

        public class Game
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string RedPlayers { get; set; }
            public string BluePlayers { get; set; }
            public int RedScore { get; set; }
            public int BlueScore { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.EntityFrameworkCore;

namespace Wuzlstats.ViewModels.Home
{
    public class GamesViewModel
    {
        private readonly Db _db;


        public GamesViewModel(Db db)
        {
            _db = db;
        }


        public async Task<GamesViewModel> Fill(League league)
        {
            League = league.Name;
            Games = await _db.Games.Where(x => x.LeagueId == league.Id).OrderByDescending(x => x.Date).Select(x => new Game
            {
                Id = x.Id,
                Date = x.Date,
                RedScore = x.RedScore,
                BlueScore = x.BlueScore
            }).ToListAsync();

            var allPlayers = await _db.Players.Where(x => x.LeagueId == league.Id).Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();

            foreach (var game in Games)
            {
                var positions = await _db.PlayerPositions.Where(x => x.GameId == game.Id).ToListAsync();
                game.RedPlayers = positions.Where(x => x.IsRedPosition).Select(x => allPlayers.Single(y => y.Id == x.PlayerId).Name).Aggregate("", (seed, value) => seed + ", " + value).Trim(',', ' ');
                game.BluePlayers = positions.Where(x => x.IsBluePosition)
                    .Select(x => allPlayers.Single(y => y.Id == x.PlayerId).Name)
                    .Aggregate("", (seed, value) => seed + ", " + value)
                    .Trim(',', ' ');
            }

            return this;
        }


        public string League { get; set; }
        public IEnumerable<Game> Games { get; set; }

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
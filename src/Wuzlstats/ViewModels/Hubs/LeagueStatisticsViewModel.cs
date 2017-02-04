using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Wuzlstats.ViewModels.Hubs
{
    public class LeagueStatisticsViewModel
    {
        private readonly Db _db;
        private readonly AppSettings _settings;
        private readonly ILogger _logger;

        public LeagueStatisticsViewModel(Db db, AppSettings settings, ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _db = db;
            _logger = loggerFactory.CreateLogger(typeof(LeagueStatisticsViewModel));
        }


        public async Task<LeagueStatisticsViewModel> Fill(League league)
        {
            _logger.LogTrace("Filling LeagueStatisticsViewModel ...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            daysForStatistics = _settings.DaysForStatistics;

            var gamesQuery = _db.Games.AsNoTracking().Where(x => x.LeagueId == league.Id);
            var date = DateTime.UtcNow.Date.AddDays(-_settings.DaysForStatistics);
            gamesQuery = gamesQuery.Where(x => x.Date >= date);
            var allGames = await gamesQuery.ToListAsync();
            _logger.LogTrace($"Loading {allGames.Count} games took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var allPlayers = await _db.Players.AsNoTracking().Where(x => x.LeagueId == league.Id).Select(x => new
            {
                x.Id,
                x.Name,
                x.Image
            }).ToListAsync();
            _logger.LogTrace($"Loading {allPlayers.Count} players took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var allGameIds = allGames.Select(x => x.Id).ToList();
            var allPositions = await _db.PlayerPositions
                .AsNoTracking()
                .Where(x => allGameIds.Contains(x.GameId))
                .Select(x => new
                {
                    x.GameId,
                    x.PlayerId,
                    x.Position
                }).ToListAsync();
            _logger.LogTrace($"Loading {allPositions.Count} player positions took {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var players = new List<Player>();
            var teams = new List<Team>();
            var redPlayerIds = new List<int>();
            var bluePlayerIds = new List<int>();
            var goalDifferences = new List<int>();

            foreach (var game in allGames)
            {
                // league stats
                games++;
                if (game.BlueWins)
                {
                    blueWins++;
                }
                else if (game.RedWins)
                {
                    redWins++;
                }
                blueGoals += game.BlueScore;
                redGoals += game.RedScore;
                goalDifferences.Add(Math.Max(game.BlueScore, game.RedScore) - Math.Min(game.BlueScore, game.RedScore));

                var positions = allPositions.Where(x => x.GameId == game.Id).ToList();

                // player stats
                foreach (var position in positions)
                {
                    var player = players.FirstOrDefault(x => x.id == position.PlayerId);
                    if (player == null)
                    {
                        var p = allPlayers.Single(x => x.Id == position.PlayerId);
                        player = Player.Create(p.Id, p.Name, p.Image);
                        players.Add(player);
                    }

                    if (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense)
                    {
                        bluePlayerIds.Add(position.PlayerId);
                    }
                    else if (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense)
                    {
                        redPlayerIds.Add(position.PlayerId);
                    }

                    // don't count ties
                    if (game.BlueWins && (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense))
                    {
                        player.wins++;
                    }
                    else if (game.RedWins && (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense))
                    {
                        player.wins++;
                    }
                    else if (game.RedWins &&
                             (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense))
                    {
                        player.losses++;
                    }
                    else if (game.BlueWins &&
                             (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense))
                    {
                        player.losses++;
                    }
                }

                // team stats
                if (positions.Count(x => x.Position == PlayerPositionTypes.BlueDefense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.BlueOffense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.RedDefense) == 1
                    && positions.Count(x => x.Position == PlayerPositionTypes.RedOffense) == 1)
                {
                    var redOffense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.RedOffense).PlayerId);
                    var redDefense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.RedDefense).PlayerId);
                    var blueOffense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.BlueOffense).PlayerId);
                    var blueDefense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.BlueDefense).PlayerId);

                    var redTeam = teams.FirstOrDefault(x => x.Equals(redOffense, redDefense));
                    if (redTeam == null)
                    {
                        redTeam = Team.Create(redOffense, redDefense);
                        teams.Add(redTeam);
                    }
                    var blueTeam = teams.FirstOrDefault(x => x.Equals(blueOffense, blueDefense));
                    if (blueTeam == null)
                    {
                        blueTeam = Team.Create(blueOffense, blueDefense);
                        teams.Add(blueTeam);
                    }

                    if (game.BlueWins)
                    {
                        redTeam.losses++;
                        blueTeam.wins++;
                    }
                    else if (game.RedWins)
                    {
                        redTeam.wins++;
                        blueTeam.losses++;
                    }
                }
            }

            redPlayers = redPlayerIds.Distinct().Count();
            bluePlayers = bluePlayerIds.Distinct().Count();
            bestPlayers = players.OrderByDescending(x => x.rank).Take(5).ToList();
            worstPlayers = players.OrderBy(x => x.rank).Take(5).ToList();
            bestTeams = teams.OrderByDescending(x => x.rank).Take(3).ToList();
            worstTeams = teams.OrderBy(x => x.rank).Take(3).ToList();

            if (goalDifferences.Any())
            {
                goalDifference = goalDifferences.Average(x => x);
            }

            var mostActivePlayerEntity = players.OrderByDescending(x => x.losses + x.wins).FirstOrDefault();
            mostActivePlayer = mostActivePlayerEntity != null ? mostActivePlayerEntity.name : "";

            _logger.LogTrace($"Calculating statistics took {stopwatch.ElapsedMilliseconds}ms.");

            return this;
        }


        // ReSharper disable InconsistentNaming
        public IEnumerable<Player> bestPlayers { get; set; }
        public IEnumerable<Player> worstPlayers { get; set; }
        public IEnumerable<Team> bestTeams { get; set; }
        public IEnumerable<Team> worstTeams { get; set; }

        public int games { get; set; }
        public int daysForStatistics { get; set; }
        public int blueGoals { get; set; }
        public int redGoals { get; set; }
        public int blueWins { get; set; }
        public int redWins { get; set; }
        public int bluePlayers { get; set; }
        public int redPlayers { get; set; }
        public double goalDifference { get; set; }
        public string mostActivePlayer { get; set; }

        public class Player
        {
            public int id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }

            // ReSharper disable once PossibleLossOfFraction
            public double rank => losses == 0 ? wins : (wins == 0 ? 0.1d / losses : (double)wins / losses);


            public bool Equals(Models.Player p)
            {
                return id == p.Id;
            }


            public static Player Create(int id, string name, byte[] image)
            {
                return new Player
                {
                    id = id,
                    name = name,
                    image = image == null || image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(image)
                };
            }
        }

        public class Team
        {
            public Player player1 { get; set; }
            public Player player2 { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }

            // ReSharper disable once PossibleLossOfFraction
            public double rank => losses == 0 ? wins : (wins == 0 ? 0.1d / losses : (double)wins / losses);


            public bool Equals(Player p1, Player p2)
            {
                if (player1 == null || player2 == null)
                {
                    return false;
                }

                if (p1.id <= p2.id)
                {
                    return player1.id == p1.id && player2.id == p2.id;
                }
                return player2.id == p1.id && player1.id == p2.id;
            }


            public static Team Create(Player p1, Player p2)
            {
                if (p1.id <= p2.id)
                {
                    return new Team
                    {
                        player1 = p1,
                        player2 = p2
                    };
                }
                return new Team
                {
                    player1 = p2,
                    player2 = p1
                };
            }
        }

        // ReSharper restore InconsistentNaming
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Hubs
{
    public class LeagueStatisticsViewModel
    {
        private readonly Db _db;
        private readonly AppSettings _settings;

        public LeagueStatisticsViewModel(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<LeagueStatisticsViewModel> Fill(League league)
        {
            daysForStatistics = _settings.DaysForStatistics;

            var gamesQuery = _db.Games.Where(x => x.LeagueId == league.Id);
            var date = DateTime.UtcNow.Date.AddDays(-_settings.DaysForStatistics);
            gamesQuery = gamesQuery.Where(x => x.Date >= date);

            // EF7 beta4 does not support navigation properties in queries yet
            // this complicates the code a lot, because we need joins :(

            var players = new List<Player>();
            var teams = new List<Team>();
            var redPlayerIds = new List<int>();
            var bluePlayerIds = new List<int>();

            foreach (var game in await gamesQuery.ToListAsync())
            {
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

                var positions = await (from position in _db.PlayerPositions
                                       join player in _db.Players on position.PlayerId equals player.Id
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
                        player = Player.Create(position.Player);
                        players.Add(player);
                    }

                    if (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense)
                    {
                        bluePlayerIds.Add(position.Player.Id);
                    }
                    else if (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense)
                    {
                        redPlayerIds.Add(position.Player.Id);
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
                    else if (game.RedWins && (position.Position == PlayerPositionTypes.Blue || position.Position == PlayerPositionTypes.BlueDefense || position.Position == PlayerPositionTypes.BlueOffense))
                    {
                        player.losses++;
                    }
                    else if (game.BlueWins && (position.Position == PlayerPositionTypes.Red || position.Position == PlayerPositionTypes.RedDefense || position.Position == PlayerPositionTypes.RedOffense))
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
                    var redOffense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.RedOffense).Player.Id);
                    var redDefense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.RedDefense).Player.Id);
                    var blueOffense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.BlueOffense).Player.Id);
                    var blueDefense = players.Single(x => x.id == positions.Single(y => y.Position == PlayerPositionTypes.BlueDefense).Player.Id);

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
                        blueTeam.wins++;
                    }
                }
            }


            redPlayers = redPlayerIds.Distinct().Count();
            bluePlayers = bluePlayerIds.Distinct().Count();
            bestPlayers = players.OrderByDescending(x => x.rank).Take(5).ToList();
            worstPlayers = players.OrderBy(x => x.rank).Take(5).ToList();
            bestTeams = teams.OrderByDescending(x => x.rank).Take(3).ToList();
            worstTeams = teams.OrderBy(x => x.rank).Take(3).ToList();
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


            public static Player Create(Models.Player p)
            {
                return new Player
                {
                    id = p.Id,
                    name = p.Name,
                    image = p.Image == null || p.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(p.Image)
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
                if (p1.name[0] <= p2.name[0])
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
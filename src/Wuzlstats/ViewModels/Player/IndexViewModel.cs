using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Player
{
    public class IndexViewModel
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public IndexViewModel(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<IndexViewModel> Fill(Models.Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image);
            RecentDays = _settings.DaysForStatistics;

            var league = await _db.Leagues.SingleOrDefaultAsync(x => x.Id == player.LeagueId);
            League = league.Name;

            var allPlayers = await _db.Players.Where(x => x.LeagueId == league.Id).ToDictionaryAsync(x => x.Id, x => x.Name);

            Statistics = await Calculate(player.Id, int.MaxValue, allPlayers);
            RecentStatistics = await Calculate(player.Id, RecentDays, allPlayers);

            return this;
        }


        private async Task<PlayerStatistics> Calculate(int playerId, int days, IDictionary<int, string> allPlayers)
        {
            var result = new PlayerStatistics();
            var minDate = days >= int.MaxValue ? new DateTime(2000, 1, 1) : DateTime.UtcNow.AddDays(-days);

            var positionsQuery = from position in _db.PlayerPositions
                                 join game in _db.Games on position.GameId equals game.Id
                                 where game.Date >= minDate && position.PlayerId == playerId
                                 select new
                                 {
                                     Position = position,
                                     Game = game
                                 };

            var firstDate = DateTime.MaxValue;
            var lastDate = DateTime.MinValue;
            var offenseCount = 0;
            var defenseCount = 0;
            var teamMateCount = new Dictionary<int, int>();
            var winPlayerCount = new Dictionary<int, int>();
            var lostPlayerCount = new Dictionary<int, int>();


            foreach (var x in positionsQuery)
            {
                var otherPositions = await _db.PlayerPositions.Where(y => y.GameId == x.Game.Id && y.PlayerId != playerId).ToListAsync();

                if (x.Position.Position == PlayerPositionTypes.Blue || x.Position.Position == PlayerPositionTypes.Red)
                {
                    result.SingleGames++;
                }
                else
                {
                    result.TeamGames++;
                }

                if (x.Position.IsRedPosition)
                {
                    result.GoalsScored += x.Game.RedScore;
                    result.GoalsTaken += x.Game.BlueScore;

                    if (x.Game.RedWins)
                    {
                        result.RedWins++;
                        AddOrIncrease(winPlayerCount, otherPositions.Where(y => y.IsBluePosition).Select(y => y.PlayerId));
                    }
                    else
                    {
                        result.RedLosses++;
                        AddOrIncrease(lostPlayerCount, otherPositions.Where(y => y.IsBluePosition).Select(y => y.PlayerId));
                    }
                }
                else if (x.Position.IsBluePosition)
                {
                    result.GoalsScored += x.Game.BlueScore;
                    result.GoalsTaken += x.Game.RedScore;

                    if (x.Game.BlueWins)
                    {
                        AddOrIncrease(winPlayerCount, otherPositions.Where(y => y.IsRedPosition).Select(y => y.PlayerId));
                        result.BlueWins++;
                    }
                    else
                    {
                        AddOrIncrease(lostPlayerCount, otherPositions.Where(y => y.IsRedPosition).Select(y => y.PlayerId));
                        result.BlueLosses++;
                    }
                }

                if (x.Position.Position == PlayerPositionTypes.BlueDefense || x.Position.Position == PlayerPositionTypes.RedDefense)
                {
                    defenseCount++;

                }
                else if (x.Position.Position == PlayerPositionTypes.BlueOffense || x.Position.Position == PlayerPositionTypes.RedOffense)
                {
                    offenseCount++;
                }

                if (x.Position.Position == PlayerPositionTypes.BlueDefense)
                {
                    AddOrIncrease(teamMateCount, otherPositions.Where(y => y.Position == PlayerPositionTypes.BlueOffense).Select(y => y.PlayerId).FirstOrDefault());
                }
                else if (x.Position.Position == PlayerPositionTypes.BlueOffense)
                {
                    AddOrIncrease(teamMateCount, otherPositions.Where(y => y.Position == PlayerPositionTypes.BlueDefense).Select(y => y.PlayerId).FirstOrDefault());
                }
                else if (x.Position.Position == PlayerPositionTypes.RedDefense)
                {
                    AddOrIncrease(teamMateCount, otherPositions.Where(y => y.Position == PlayerPositionTypes.RedOffense).Select(y => y.PlayerId).FirstOrDefault());
                }
                else if (x.Position.Position == PlayerPositionTypes.RedOffense)
                {
                    AddOrIncrease(teamMateCount, otherPositions.Where(y => y.Position == PlayerPositionTypes.RedDefense).Select(y => y.PlayerId).FirstOrDefault());
                }

                if (x.Game.Date < firstDate)
                {
                    firstDate = x.Game.Date;
                }
                if (x.Game.Date > lastDate)
                {
                    lastDate = x.Game.Date;
                }
            }


            if (offenseCount > defenseCount)
            {
                result.FavoritePosition = "Offense";
                result.FavoritePositionGames = offenseCount;
            }
            else if (offenseCount < defenseCount)
            {
                result.FavoritePosition = "Defense";
                result.FavoritePositionGames = defenseCount;
            }
            else if (offenseCount > 0 && defenseCount > 0 && offenseCount == defenseCount)
            {
                result.FavoritePosition = "Awesome everywhere";
                result.FavoritePositionGames = offenseCount;
            }

            if (lastDate > DateTime.MinValue)
            {
                result.LastGameDate = lastDate.ToString("dd MMMM yyyy, HH:mm") + " UTC";
            }
            if (firstDate < DateTime.MaxValue)
            {
                result.FirstGameDate = firstDate.ToString("dd MMMM yyyy, HH:mm") + " UTC";
            }

            int key;
            if (teamMateCount.Any())
            {
                key = teamMateCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (allPlayers.ContainsKey(key))
                {
                    result.PlayedMostGamesWith = allPlayers[key];
                    result.PlayedMostGamesWithCount = teamMateCount[key];
                }
            }
            if (winPlayerCount.Any())
            {
                key = winPlayerCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (allPlayers.ContainsKey(key))
                {
                    result.WonMostGamesAgainst = allPlayers[key];
                    result.WonMostGamesAgainstCount = winPlayerCount[key];
                }
            }
            if (lostPlayerCount.Any())
            {
                key = lostPlayerCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (allPlayers.ContainsKey(key))
                {
                    result.LostMostGamesAgainst = allPlayers[key];
                    result.LostMostGamesAgainstCount = lostPlayerCount[key];
                }
            }


            return result;
        }


        private void AddOrIncrease(IDictionary<int, int> dictionary, int key)
        {
            if (key <= 0)
            {
                return;
            }

            if (dictionary.ContainsKey(key))
            {
                dictionary[key]++;
            }
            else
            {
                dictionary[key] = 1;
            }
        }


        private void AddOrIncrease(IDictionary<int, int> dictionary, IEnumerable<int> keys)
        {
            foreach (var key in keys)
            {
                AddOrIncrease(dictionary, key);
            }
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string League { get; set; }
        public string Image { get; set; }
        public int RecentDays { get; set; }

        public PlayerStatistics Statistics { get; set; }
        public PlayerStatistics RecentStatistics { get; set; }

        public class PlayerStatistics
        {
            public int SingleGames { get; set; }
            public int TeamGames { get; set; }
            public int GoalsScored { get; set; }
            public int GoalsTaken { get; set; }
            public string FirstGameDate { get; set; }
            public string LastGameDate { get; set; }
            public int BlueWins { get; set; }
            public int BlueLosses { get; set; }
            public int RedWins { get; set; }
            public int RedLosses { get; set; }
            public int FavoritePositionGames { get; set; }
            public string FavoritePosition { get; set; }
            public string PlayedMostGamesWith { get; set; }
            public int PlayedMostGamesWithCount { get; set; }
            public string WonMostGamesAgainst { get; set; }
            public int WonMostGamesAgainstCount { get; set; }
            public string LostMostGamesAgainst { get; set; }
            public int LostMostGamesAgainstCount { get; set; }
        }

    }
}
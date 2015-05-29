using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Api
{
    public class TeamRankingViewModel
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public TeamRankingViewModel(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<TeamRankingViewModel> Fill(League league, int count)
        {
            var date = DateTime.UtcNow.Date.AddDays(-_settings.TeamRankingDays);
            var gamesQuery = _db.Games.Where(x => x.LeagueId == league.Id && x.Date >= date);

            // EF7 beta4 does not support navigation properties in queries yet
            // this complicates the code a lot, because we need joins :(

            var result = new List<Team>();
            foreach (var game in await gamesQuery.ToListAsync())
            {
                var redOffense = await (from position in _db.PlayerPositions
                                        join player in _db.Players on position.PlayerId equals player.Id
                                        where position.GameId == game.Id && position.Position == PlayerPositionTypes.RedOffense
                                        select player).FirstOrDefaultAsync();
                var redDefense = await (from position in _db.PlayerPositions
                                        join player in _db.Players on position.PlayerId equals player.Id
                                        where position.GameId == game.Id && position.Position == PlayerPositionTypes.RedDefense
                                        select player).FirstOrDefaultAsync();
                var blueOffense = await (from position in _db.PlayerPositions
                                         join player in _db.Players on position.PlayerId equals player.Id
                                         where position.GameId == game.Id && position.Position == PlayerPositionTypes.BlueOffense
                                         select player).FirstOrDefaultAsync();
                var blueDefense = await (from position in _db.PlayerPositions
                                         join player in _db.Players on position.PlayerId equals player.Id
                                         where position.GameId == game.Id && position.Position == PlayerPositionTypes.BlueDefense
                                         select player).FirstOrDefaultAsync();

                if (redOffense != null && redDefense != null && blueOffense != null && blueDefense != null)
                {
                    var redTeam = result.FirstOrDefault(x => x.Equals(redOffense, redDefense));
                    if (redTeam == null)
                    {
                        redTeam = Team.Create(redOffense, redDefense);
                        result.Add(redTeam);
                    }
                    var blueTeam = result.FirstOrDefault(x => x.Equals(redOffense, redDefense));
                    if (blueTeam == null)
                    {
                        blueTeam = Team.Create(blueOffense, blueDefense);
                        result.Add(blueTeam);
                    }

                    if (game.BlueScore > game.RedScore)
                    {
                        redTeam.losses++;
                        blueTeam.wins++;
                    }
                    else if (game.RedScore > game.BlueScore)
                    {
                        redTeam.wins++;
                        blueTeam.wins++;
                    }
                }
            }

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (count < 0)
            {
                teams = result.OrderBy(x => x.rank).Take(-count).ToList();
            }
            else
            {
                teams = result.OrderByDescending(x => x.rank).Take(count).ToList();
            }

            return this;
        }

        // ReSharper disable InconsistentNaming
        public IEnumerable<Team> teams { get; set; }

        public class Team
        {
            public int id1 { get; set; }
            public string name1 { get; set; }
            public string image1 { get; set; }
            public int id2 { get; set; }
            public string name2 { get; set; }
            public string image2 { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }

            // ReSharper disable once PossibleLossOfFraction
            public double rank => losses == 0 ? wins : (wins == 0 ? 0.1 / losses : wins / losses);


            public bool Equals(Models.Player p1, Models.Player p2)
            {
                if (p1.Id <= p2.Id)
                {
                    return id1 == p1.Id && id2 == p2.Id;
                }
                return id2 == p1.Id && id1 == p2.Id;
            }


            public static Team Create(Models.Player p1, Models.Player p2)
            {
                if (p1.Name[0] <= p2.Name[0])
                {
                    return new Team
                    {
                        id1 = p1.Id,
                        id2 = p2.Id,
                        name1 = p1.Name,
                        name2 = p2.Name,
                        image1 = p1.Image == null || p1.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(p1.Image),
                        image2 = p2.Image == null || p2.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(p2.Image)
                    };
                }
                return new Team
                {
                    id1 = p2.Id,
                    id2 = p1.Id,
                    name1 = p2.Name,
                    name2 = p1.Name,
                    image1 = p2.Image == null || p2.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(p2.Image),
                    image2 = p1.Image == null || p1.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(p1.Image)
                };
            }
        }
        // ReSharper restore InconsistentNaming
    }
}
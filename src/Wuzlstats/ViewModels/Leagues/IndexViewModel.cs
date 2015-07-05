using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.Data.Entity;

namespace Wuzlstats.ViewModels.Leagues
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


        public async Task<IndexViewModel> Fill()
        {
            // join doesn't work yet with EF7

            var minDate = DateTime.UtcNow.Date.AddDays(-_settings.DaysForStatistics);
            Leagues = await _db.Leagues.OrderBy(x => x.Name).Select(x => new League()
            {
                Name = x.Name,
                GamesCountTotal = _db.Games.Count(y => y.LeagueId == x.Id),
                GamesCountDays = _db.Games.Count(y => y.LeagueId == x.Id && y.Date >= minDate)
            }).ToListAsync();

            DaysForStatistics = _settings.DaysForStatistics;
            return this;
        }


        public IEnumerable<League> Leagues { get; set; }
        public int DaysForStatistics { get; set; }

        public class League
        {
            public string Name { get; set; }
            public int GamesCountTotal { get; set; }
            public int GamesCountDays { get; set; }
        }
    }
}
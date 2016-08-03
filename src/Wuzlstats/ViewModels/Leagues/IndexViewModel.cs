using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.EntityFrameworkCore;

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
                Id = x.Id,
                GamesCountTotal = 0,
                GamesCountDays = 0
            }).ToListAsync();

            // for some strange reason (probably bug in EF), we can't read these properties directly in the query above (there's and IooB exception)

            foreach (var league in Leagues)
            {
                league.GamesCountTotal = _db.Games.Count(x => x.LeagueId == league.Id);
                league.GamesCountDays = _db.Games.Count(x => x.LeagueId == league.Id && x.Date >= minDate);
            }
            


            DaysForStatistics = _settings.DaysForStatistics;
            return this;
        }


        public IEnumerable<League> Leagues { get; set; }
        public int DaysForStatistics { get; set; }

        public class League
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int GamesCountTotal { get; set; }
            public int GamesCountDays { get; set; }
        }
    }
}
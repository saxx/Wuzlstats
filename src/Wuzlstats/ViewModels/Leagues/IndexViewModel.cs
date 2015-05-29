using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Leagues
{
    public class IndexViewModel
    {
        private readonly Db _db;

        public IndexViewModel(Db db)
        {
            _db = db;
        }
        
        public async Task<IndexViewModel> Fill()
        {
            Leagues = await _db.Leagues.OrderBy(x => x.Name).Select(x => new League()
            {
                Name = x.Name
            }).ToListAsync();

            return this;
        }

        public IEnumerable<League> Leagues { get; set; }
        
        public class League
        {
            public string Name { get; set; }
        } 
    }
}
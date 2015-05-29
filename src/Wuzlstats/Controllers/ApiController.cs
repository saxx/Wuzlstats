using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Wuzlstats.ExtensionMethods;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Api;

namespace Wuzlstats.Controllers
{
    [Route("Api")]
    public class ApiController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public ApiController(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        [HttpPost("{league}/Score")]
        public async Task<IActionResult> Score(string league, ScoreViewModel viewModel)
        {
            if (league.IsNoE())
            {
                throw new Exception("Invalid league, must not be empty.");
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.Equals(league, StringComparison.CurrentCultureIgnoreCase));
            if (leagueEntity == null)
            {
                throw new Exception("Invalid league.");
            }

            await viewModel.Save(leagueEntity, _db);

            return Json(viewModel);
        }

        [HttpGet("{league}/PlayerRanking/{count}")]
        public async Task<IActionResult> PlayerRanking(string league, int count)
        {
            if (league.IsNoE())
            {
                throw new Exception("Invalid league, must not be empty.");
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.Equals(league, StringComparison.CurrentCultureIgnoreCase));
            if (leagueEntity == null)
            {
                throw new Exception("Invalid league.");
            }

            return Json(await new PlayerRankingViewModel(_db, _settings).Fill(leagueEntity, count));
        }

        [HttpGet("{league}/TeamRanking/{count}")]
        public async Task<IActionResult> TeamRanking(string league, int count)
        {
            if (league.IsNoE())
            {
                throw new Exception("Invalid league, must not be empty.");
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.Equals(league, StringComparison.CurrentCultureIgnoreCase));
            if (leagueEntity == null)
            {
                throw new Exception("Invalid league.");
            }

            return Json(await new TeamRankingViewModel(_db, _settings).Fill(leagueEntity, count));
        }
    }
}
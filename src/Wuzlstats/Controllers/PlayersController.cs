using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Players;
using System.Threading.Tasks;
using Wuzlstats.Services;

namespace Wuzlstats.Controllers
{
    public class PlayersController : Controller
    {
        private readonly Db _db;
        private readonly LeagueHelper _leagueHelper;
        private readonly PlayersService _statisticsService;

        public PlayersController(Db db, LeagueHelper leagueHelper)
        {
            _leagueHelper = leagueHelper;
            _db = db;
            _statisticsService = new PlayersService(_db);
        }

        [Route("~/League/{league}/Players")]
        public async Task<IActionResult> Index(string league, string sort, bool recent)
        {
            var leagueEntity = _db.Leagues.FirstOrDefault(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return RedirectToAction("Index", "Leagues");
            }
            ViewBag.CurrentLeague = leagueEntity.Name;
            ViewBag.CurrentLeagueColors = _leagueHelper.GenerateCssVariables(leagueEntity);
            ViewBag.CurrentLeagueBanner = leagueEntity.BannerImageUrl;
            ViewBag.CurrentLeagueDescription = leagueEntity.Description;

            var days = _leagueHelper.GetDaysForStatistics(leagueEntity);
            var players = await _statisticsService.FindPlayersOfLeague(leagueEntity.Id, recent ? days : default(int?));

            switch (sort)
            {
                case "best":
                    players = players.OrderByDescending(x => x.Score);
                    break;
                case "worst":
                    players = players.OrderBy(x => x.Score);
                    break;
                case "activity":
                    players = players.OrderByDescending(x => x.Wins + x.Losses);
                    break;
                default:
                    break;
            }

            return View(new IndexViewModel
            {
                ActiveFilter = sort,
                Recent = recent,
                Days = days,
                Players = players
            });
        }

    }
}

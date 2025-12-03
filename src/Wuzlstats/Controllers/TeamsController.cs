using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wuzlstats.Models;
using Wuzlstats.Services;
using Wuzlstats.ViewModels.Teams;

namespace Wuzlstats.Controllers
{
    public class TeamsController : Controller
    {
        private readonly Db _db;
        private readonly LeagueHelper _leagueHelper;
        private readonly TeamStatisticsService _statisticsService;

        public TeamsController(Db db, LeagueHelper leagueHelper)
        {
            _leagueHelper = leagueHelper;
            _db = db;
            _statisticsService = new TeamStatisticsService(_db);
        }

        [Route("~/League/{league}/Teams")]
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
            var teams = await _statisticsService.FindTeamsOfLeagueAsync(leagueEntity.Id, recent ? days : default(int?));

            switch (sort)
            {
                case "best":
                    teams = teams.OrderByDescending(x => x.Rank);
                    break;
                case "worst":
                    teams = teams.OrderBy(x => x.Rank);
                    break;
                case "activity":
                    teams = teams.OrderByDescending(x => x.Wins + x.Losses);
                    break;
                default:
                    teams = teams.OrderByDescending(x => x.LastGamePlayedOn);
                    break;
            }

            return View(new IndexViewModel
            {
                ActiveFilter = sort,
                Recent = recent,
                Days = days,
                Teams = teams
            });
        }
    }
}

using System.Linq;
using Microsoft.AspNet.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Players;
using System;
using System.Threading.Tasks;
using Wuzlstats.Services;

namespace Wuzlstats.Controllers
{
    public class PlayersController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;
        private readonly PlayersService _statisticsService;

        public PlayersController(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
            _statisticsService = new PlayersService(_db);
        }

        [Route("~/League/{league}/Players")]
        public async Task<IActionResult> Index(string league, string sort)
        {
            var leagueEntity = _db.Leagues.FirstOrDefault(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return RedirectToAction("Index", "Leagues");
            }
            ViewBag.CurrentLeague = leagueEntity.Name;
            var players = await _statisticsService.FindPlayersOfLeague(leagueEntity.Id, /*_settings.DaysForStatistics*/ null);

            switch (sort)
            {
                case "best":
                    players = players.OrderByDescending(x => x.Rank);
                    break;
                case "worst":
                    players = players.OrderBy(x => x.Rank);
                    break;
                case "activity":
                    players = players.OrderByDescending(x => x.Wins + x.Losses);
                    break;
                default:
                    break;
            }

            return View(players.Select(player => new IndexViewModel
            {
                PlayerId = player.Id,
                Name = player.Name,
                Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image),
                Wins = player.Wins,
                Losses = player.Losses,
                SingleGames = player.SingleGames,
                TeamGames = player.TeamGames,
                LastGamePlayedOn = player.LatestGame
            }));
        }

    }
}

using System.Linq;
using Microsoft.AspNet.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Players;
using System;

namespace Wuzlstats.Controllers
{
    public class PlayersController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;

        public PlayersController(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }

        [Route("~/League/{league}/Players")]
        public IActionResult Index(string league)
        {
            var leagueEntity = _db.Leagues.FirstOrDefault(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return RedirectToAction("Index", "Leagues");
            }
            ViewBag.CurrentLeague = leagueEntity.Name;
            var players = _db.Players.Where(x => x.LeagueId == leagueEntity.Id).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).ToList();

            return View(players.Select(player => new IndexViewModel
            {
                PlayerId = player.Id,
                Name = player.Name,
                Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image)
            }));
        }
    }
}

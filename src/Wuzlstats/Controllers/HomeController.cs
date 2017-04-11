using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Wuzlstats.Extensions;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Home;

namespace Wuzlstats.Controllers
{
    public class HomeController : Controller
    {
        private readonly Db _db;
        private readonly IServiceProvider _services;


        public HomeController(Db db, IServiceProvider services)
        {
            _db = db;
            _services = services;
        }


        [Route("~/League/{league}")]
        public IActionResult Index(string league)
        {
            Response.Cookies.Append("league", league, new CookieOptions
            {
                Expires = DateTime.MaxValue
            });
            return Redirect("~/");
        }


        public async Task<IActionResult> Index()
        {
            var leagueCookie = Request.Cookies["league"];
            if (StringValues.IsNullOrEmpty(leagueCookie))
            {
                return RedirectToAction("Index", "Leagues");
            }

            var league = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == leagueCookie.ToString().ToLower());
            if (league == null)
            {
                return RedirectToAction("Index", "Leagues");
            }

            ViewBag.CurrentLeague = league.Name;
            return View(new IndexViewModel().Fill(league));
        }


        [Route("~/League/{league}/Games")]
        public async Task<IActionResult> Games(string league)
        {
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return RedirectToAction("Index", "Leagues");
            }

            ViewBag.CurrentLeague = leagueEntity.Name;
            return View(await _services.GetRequiredService<GamesViewModel>().Fill(leagueEntity));
        }


        [HttpGet("~/League/{league}/Games/{game}/Delete")]
        public async Task<IActionResult> DeleteGame(string league, int? game)
        {
            if (league.IsNullOrEmpty() || !game.HasValue)
            {
                return NotFound();
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return NotFound();
            }
            var gameEntity = await _db.Games.Include(include => include.Positions).FirstOrDefaultAsync(x => (x.Id == game) && (x.LeagueId == leagueEntity.Id));
            if (gameEntity == null)
            {
                return NotFound();
            }
            var players = await _db.Players.Where(player => gameEntity.Positions.Select(x => x.PlayerId).Contains(player.Id)).ToDictionaryAsync(x => x.Id, x => x.Name);

            var bluePlayerIds = gameEntity.Positions.Where(x => x.IsBluePosition).Select(x => x.PlayerId).ToList();
            var redPlayerIds = gameEntity.Positions.Where(x => x.IsRedPosition).Select(x => x.PlayerId).ToList();
            return View(new DeleteGameViewModel
            {
                Id = gameEntity.Id,
                PlayedOn = gameEntity.Date,
                BluePlayers = BuildPlayerNames(FetchPlayers(players, bluePlayerIds)),
                RedPlayers = BuildPlayerNames(FetchPlayers(players, redPlayerIds)),
                RedScore = gameEntity.RedScore,
                BlueScore = gameEntity.BlueScore
            });
        }


        private static IEnumerable<string> FetchPlayers(IDictionary<int, string> allPlayers, IEnumerable<int> playerIds)
        {
            return playerIds.Select(playerId => allPlayers[playerId]).ToList();
        }


        private static string BuildPlayerNames(IEnumerable<string> players)
        {
            return string.Join(", ", players);
        }


        [HttpPost("~/League/{league}/Games/{game}/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGameConfirmed(string league, int game)
        {
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity != null)
            {
                var gameEntity = await _db.Games.FirstOrDefaultAsync(x => (x.Id == game) && (x.LeagueId == leagueEntity.Id));
                if (gameEntity != null)
                {
                    _db.PlayerPositions.RemoveRange(_db.PlayerPositions.Where(x => x.GameId == gameEntity.Id));
                    _db.Games.Remove(gameEntity);
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Games");
        }


        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

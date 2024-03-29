﻿using Wuzlstats.Models;
using Wuzlstats.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Wuzlstats.Controllers
{
    public class HomeController : Controller
    {
        private readonly Db _db;


        public HomeController(Db db)
        {
            _db = db;
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
            if (leagueCookie == null)
            {
                return RedirectToAction("Index", "Leagues");
            }

            var league = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == leagueCookie.ToLower());
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
            return View(await new GamesViewModel(_db).Fill(leagueEntity));
        }


        [HttpGet("~/League/{league}/Games/{game}/Delete")]
        public async Task<IActionResult> DeleteGame(string league, int game)
        {
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity != null)
            {
                var gameEntity = await _db.Games.FirstOrDefaultAsync(x => x.Id == game && x.LeagueId == leagueEntity.Id);
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

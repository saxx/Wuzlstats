using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Home;
using Microsoft.Extensions.Primitives;

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
            return View(await new GamesViewModel(_db).Fill(leagueEntity));
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

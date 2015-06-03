using System;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Home;

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

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

using System.Linq;
using Microsoft.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Leagues;

namespace Wuzlstats.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public LeaguesController(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            var viewModel = await new IndexViewModel(_db, _settings).Fill();
            return View(viewModel);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            name = (name ?? "").Trim();
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError(nameof(name), "Name is required.");
            }
            else if (await _db.Leagues.AnyAsync(x => x.Name.ToLower() == name.ToLower()))
            {
                ModelState.AddModelError(nameof(name), "Name already exists.");
            }
            else
            {
                _db.Leagues.Add(new League
                {
                    Name = name
                });
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { league = name });
            }
            return View();
        }
    }
}
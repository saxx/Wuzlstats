using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
        [Route("~/League/{league}/Settings")]
        public async Task<IActionResult> Edit(string league, string? password)
        {
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return NotFound();
            }

            ViewBag.CurrentLeague = leagueEntity.Name;

            // Password protection check
            if (!string.IsNullOrEmpty(leagueEntity.PasswordHash))
            {
                if (string.IsNullOrEmpty(password))
                {
                    return View("EnterPassword", new EnterPasswordViewModel { LeagueName = league });
                }

                if (!BCrypt.Net.BCrypt.Verify(password, leagueEntity.PasswordHash))
                {
                    ModelState.AddModelError("", "Incorrect password");
                    return View("EnterPassword", new EnterPasswordViewModel { LeagueName = league });
                }
            }

            return View(EditViewModel.FromLeague(leagueEntity));
        }


        [HttpPost]
        [Route("~/League/{league}/Settings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string league, EditViewModel model)
        {
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                return NotFound();
            }

            ViewBag.CurrentLeague = leagueEntity.Name;

            // Password verification (if set)
            if (!string.IsNullOrEmpty(leagueEntity.PasswordHash))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword) ||
                    !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, leagueEntity.PasswordHash))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "Incorrect password");
                    model.HasPassword = true;
                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                model.HasPassword = !string.IsNullOrEmpty(leagueEntity.PasswordHash);
                return View(model);
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Passwords do not match");
                    model.HasPassword = !string.IsNullOrEmpty(leagueEntity.PasswordHash);
                    return View(model);
                }
                leagueEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            }

            // Apply settings
            model.ApplyToLeague(leagueEntity);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "League settings updated successfully!";
            return RedirectToAction("Index", "Home", new { league = leagueEntity.Name });
        }
    }
}
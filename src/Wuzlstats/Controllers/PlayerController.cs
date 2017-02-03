using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ImageSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Player;
using ImageSharp.Formats;

namespace Wuzlstats.Controllers
{
    public class PlayerController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public PlayerController(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }


        public async Task<IActionResult> Index(int id)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            var viewModel = await new IndexViewModel(_db, _settings).Fill(player);
            ViewBag.CurrentLeague = viewModel.League;
            return View(viewModel);
        }

        #region Avatar
        public async Task<IActionResult> Avatar(int id)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            var viewModel = await new AvatarViewModel(_db).Fill(player);
            ViewBag.CurrentLeague = viewModel.League;
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Avatar(int id, IFormFile avatar)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            if (avatar.Length > 0)
            {
                var outputStream = new MemoryStream();
                using (Image image = new Image(avatar.OpenReadStream()))
                {
                    image.Resize(150, 150).Save(outputStream, new PngFormat());
                }
                player.Image = outputStream.ToArray();
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { id });
        }
        #endregion


        private async Task<Player> LoadAndEnsurePlayerExists(int id)
        {
            var player = await _db.Players.FirstOrDefaultAsync(x => x.Id == id);
            if (player == null)
            {
                throw new Exception("There is no player with ID " + id + ".");
            }
            return player;
        }
    }
}

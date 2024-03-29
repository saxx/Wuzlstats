﻿using Wuzlstats.Models;
using Wuzlstats.ViewModels.Player;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

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
                var image = SKBitmap.Decode(avatar.OpenReadStream());
                var maxWidth = 150;
                var maxHeight = 150;
                var format = SKEncodedImageFormat.Png;

                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var info = new SKImageInfo(newWidth, newHeight);
                image = image.Resize(info, SKFilterQuality.High);

                using var outputStream = new MemoryStream();

                image.Encode(outputStream, format, 100);
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

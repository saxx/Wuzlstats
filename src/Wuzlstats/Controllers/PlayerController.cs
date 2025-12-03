using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Player;
using Wuzlstats.Services;
using SkiaSharp;

namespace Wuzlstats.Controllers
{
    public class PlayerController : Controller
    {
        private readonly Db _db;
        private readonly AppSettings _settings;
        private readonly LeagueHelper _leagueHelper;


        public PlayerController(Db db, AppSettings settings, LeagueHelper leagueHelper)
        {
            _settings = settings;
            _db = db;
            _leagueHelper = leagueHelper;
        }


        public async Task<IActionResult> Index(int id)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            var viewModel = await new IndexViewModel(_db, _settings).Fill(player);

            var league = await _db.Leagues.FirstOrDefaultAsync(x => x.Id == player.LeagueId);
            if (league != null)
            {
                ViewBag.CurrentLeague = league.Name;
                ViewBag.CurrentLeagueColors = _leagueHelper.GenerateCssVariables(league);
                ViewBag.CurrentLeagueBanner = league.BannerImageUrl;
                ViewBag.CurrentLeagueDescription = league.Description;
            }
            else
            {
                ViewBag.CurrentLeague = viewModel.League;
            }

            return View(viewModel);
        }

        #region Avatar
        public async Task<IActionResult> Avatar(int id)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            var viewModel = await new AvatarViewModel(_db).Fill(player);

            var league = await _db.Leagues.FirstOrDefaultAsync(x => x.Id == player.LeagueId);
            if (league != null)
            {
                ViewBag.CurrentLeague = league.Name;
                ViewBag.CurrentLeagueColors = _leagueHelper.GenerateCssVariables(league);
                ViewBag.CurrentLeagueBanner = league.BannerImageUrl;
                ViewBag.CurrentLeagueDescription = league.Description;
            }
            else
            {
                ViewBag.CurrentLeague = viewModel.League;
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Avatar(int id, IFormFile avatar)
        {
            var player = await LoadAndEnsurePlayerExists(id);
            if (avatar != null && avatar.Length > 0)
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

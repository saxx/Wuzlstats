using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageResizer;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Player;

namespace Wuzlstats.Controllers
{
    public class PlayerController : Controller
    {
        private readonly Db _db;


        public PlayerController(Db db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index(int id)
        {
            var viewModel = await new IndexViewModel(_db).Fill(id);
            ViewBag.CurrentLeague = viewModel.League;
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Index(int id, IFormFile avatar)
        {
            var player = await _db.Players.SingleOrDefaultAsync(x => x.Id == id);
            if (player != null && avatar.Length > 0)
            {
                var settings = new ResizeSettings
                {
                    MaxWidth = 150,
                    MaxHeight = 150,
                    Format = "png"
                };

                var outputStream = new MemoryStream();
                ImageBuilder.Current.Build(avatar.OpenReadStream(), outputStream, settings);

                player.Image = outputStream.ToArray();
                await _db.SaveChangesAsync();
            }
            return await Index(id);
        }
    }
}
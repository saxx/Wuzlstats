using System;
using System.Linq;
using System.Threading.Tasks;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Player
{
    public class IndexViewModel
    {
        private readonly Db _db;
        
        public IndexViewModel(Db db)
        {
            _db = db;
        }


        public async Task<IndexViewModel> Fill(int id)
        {
            var player = await _db.Players.SingleOrDefaultAsync(x => x.Id == id);
            if (player == null)
            {
                throw new Exception("There is no player with ID " + id + ".");
            }

            Id = player.Id;
            Name = player.Name;
            Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image);

            return this;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
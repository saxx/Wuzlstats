using System;
using System.Threading.Tasks;
using Wuzlstats.Models;
using Microsoft.Data.Entity;

namespace Wuzlstats.ViewModels.Player
{
    public class AvatarViewModel
    {
        private readonly Db _db;
        
        public AvatarViewModel(Db db)
        {
            _db = db;
        }


        public async Task<AvatarViewModel> Fill(Models.Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Image = player.Image == null || player.Image.Length <= 0 ? EmptyAvatar.Base64 : Convert.ToBase64String(player.Image);

            var league = await _db.Leagues.SingleOrDefaultAsync(x => x.Id == player.LeagueId);
            League = league.Name;

            return this;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string League { get; set; }
        public string Image { get; set; }
    }
}
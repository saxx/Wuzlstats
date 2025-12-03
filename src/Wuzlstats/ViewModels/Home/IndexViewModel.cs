using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Home
{
    public class IndexViewModel
    {
        public IndexViewModel Fill(League league)
        {
            Name = league.Name;
            Description = league.Description;
            BannerImageUrl = league.BannerImageUrl;

            return this;
        }


        public string Name { get; set; }
        public string? Description { get; set; }
        public string? BannerImageUrl { get; set; }
    }
}
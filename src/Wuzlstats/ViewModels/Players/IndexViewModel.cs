using System;

namespace Wuzlstats.ViewModels.Players
{
    public class IndexViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public DateTime LastGamePlayedOn { get; set; }
        public int SingleGames { get; set; }
        public int TeamGames { get; set; }
    }
}

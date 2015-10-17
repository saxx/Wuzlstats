using System;

namespace Wuzlstats.ViewModels.Players
{
    public class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public DateTime LastGamePlayedOn { get; set; }
        public int SingleGames { get; set; }
        public int TeamGames { get; set; }

        public double Score => Losses == 0 ? Wins : (Wins == 0 ? 0.1d / Losses : (double)Wins / Losses);
    }
}

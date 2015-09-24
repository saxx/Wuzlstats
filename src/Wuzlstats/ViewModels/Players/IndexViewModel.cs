using System;

namespace Wuzlstats.ViewModels.Players
{
    public class IndexViewModel
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime LastGamePlayedOn { get; set; }
    }
}

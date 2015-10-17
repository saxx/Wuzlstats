using System.Collections.Generic;

namespace Wuzlstats.ViewModels.Players
{
    public class IndexViewModel
    {
        public string ActiveFilter { get; set; }
        public bool Recent { get; set; }
        public int Days { get; set; }
        public IEnumerable<PlayerViewModel> Players { get; set; }
    }
}

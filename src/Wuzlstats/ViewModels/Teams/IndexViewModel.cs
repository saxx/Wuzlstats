using System.Collections.Generic;

namespace Wuzlstats.ViewModels.Teams
{
    public class IndexViewModel
    {
        public string ActiveFilter { get; set; }
        public bool Recent { get; set; }
        public int Days { get; set; }
        public IEnumerable<TeamViewModel> Teams { get; set; }
    }
}

using System.Collections.Generic;

namespace Wuzlstats.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? TimeoutConfiguration { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}

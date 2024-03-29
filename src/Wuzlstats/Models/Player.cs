using System.Collections.Generic;

namespace Wuzlstats.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public byte[]? Image { get; set; }

        public League League { get; set; }
        public ICollection<PlayerPosition> Positions { get; set; }
    }
}

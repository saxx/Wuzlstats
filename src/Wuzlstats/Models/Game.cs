using System;
using System.Collections.Generic;

namespace Wuzlstats.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public DateTime Date { get; set; }
        public int BlueScore { get; set; }
        public int RedScore { get; set; }

        public virtual League League { get; set; }
        public virtual ICollection<PlayerPosition> Positions { get; set; }
    }
}
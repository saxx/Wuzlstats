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

        public League League { get; set; }
        public ICollection<PlayerPosition> Positions { get; set; }

        public bool BlueWins => BlueScore > RedScore;
        public bool RedWins => BlueScore < RedScore;
    }
}

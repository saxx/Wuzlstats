using System.Collections.Generic;

namespace Wuzlstats.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? TimeoutConfiguration { get; set; }

        // Team Colors
        public string TeamBlueColor { get; set; } = "#ddeeff";
        public string TeamRedColor { get; set; } = "#ffddee";

        // Statistics Configuration
        public int? DaysForStatistics { get; set; }  // Nullable - null means use global default

        // Game Scoring Rules
        public int MaxScore { get; set; } = 10;

        // Display Preferences
        public string? Description { get; set; }
        public string? BannerImageUrl { get; set; }

        // Statistical Preferences
        public bool ShowPlayerRankings { get; set; } = true;
        public bool ShowTeamRankings { get; set; } = true;
        public int MinimumGamesForRanking { get; set; } = 5;

        // Security
        public string? PasswordHash { get; set; }  // BCrypt hash

        public ICollection<Game> Games { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Wuzlstats.Models;

namespace Wuzlstats.ViewModels.Leagues
{
    public class EditViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = "";

        // Password protection
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
        public bool HasPassword { get; set; }

        // Team Colors
        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Must be valid hex color")]
        public string TeamBlueColor { get; set; } = "#ddeeff";

        [Required]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Must be valid hex color")]
        public string TeamRedColor { get; set; } = "#ffddee";

        // Statistics
        [Range(1, 1000, ErrorMessage = "Days must be between 1 and 1000")]
        public int? DaysForStatistics { get; set; }

        // Game Rules
        [Range(1, 20, ErrorMessage = "Max score must be between 1 and 20")]
        public int MaxScore { get; set; } = 10;

        // Display
        [MaxLength(500)]
        public string? Description { get; set; }

        [Url]
        public string? BannerImageUrl { get; set; }

        // Statistical Preferences
        public bool ShowPlayerRankings { get; set; } = true;
        public bool ShowTeamRankings { get; set; } = true;

        [Range(0, 100)]
        public int MinimumGamesForRanking { get; set; } = 5;

        public static EditViewModel FromLeague(League league)
        {
            return new EditViewModel
            {
                LeagueId = league.Id,
                LeagueName = league.Name,
                HasPassword = !string.IsNullOrEmpty(league.PasswordHash),
                TeamBlueColor = league.TeamBlueColor,
                TeamRedColor = league.TeamRedColor,
                DaysForStatistics = league.DaysForStatistics,
                MaxScore = league.MaxScore,
                Description = league.Description,
                BannerImageUrl = league.BannerImageUrl,
                ShowPlayerRankings = league.ShowPlayerRankings,
                ShowTeamRankings = league.ShowTeamRankings,
                MinimumGamesForRanking = league.MinimumGamesForRanking
            };
        }

        public void ApplyToLeague(League league)
        {
            league.TeamBlueColor = TeamBlueColor;
            league.TeamRedColor = TeamRedColor;
            league.DaysForStatistics = DaysForStatistics;
            league.MaxScore = MaxScore;
            league.Description = Description;
            league.BannerImageUrl = BannerImageUrl;
            league.ShowPlayerRankings = ShowPlayerRankings;
            league.ShowTeamRankings = ShowTeamRankings;
            league.MinimumGamesForRanking = MinimumGamesForRanking;
        }
    }
}

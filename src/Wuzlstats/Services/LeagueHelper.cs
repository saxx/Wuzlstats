using System;
using Wuzlstats.Models;

namespace Wuzlstats.Services
{
    public class LeagueHelper
    {
        private readonly AppSettings _appSettings;

        public LeagueHelper(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public int GetDaysForStatistics(League league)
        {
            return league.DaysForStatistics ?? _appSettings.DaysForStatistics;
        }

        public bool ShouldShowPlayerRankings(League league)
        {
            return league.ShowPlayerRankings;
        }

        public bool ShouldShowTeamRankings(League league)
        {
            return league.ShowTeamRankings;
        }

        public int GetMinimumGamesForRanking(League league)
        {
            return league.MinimumGamesForRanking;
        }

        public string GenerateCssVariables(League league)
        {
            var blueFocus = GenerateFocusColor(league.TeamBlueColor);
            var redFocus = GenerateFocusColor(league.TeamRedColor);
            var blueText = GetTextColor(league.TeamBlueColor);
            var redText = GetTextColor(league.TeamRedColor);

            return $@":root {{
    --team-blue: {league.TeamBlueColor};
    --team-red: {league.TeamRedColor};
    --team-blue-focus: {blueFocus};
    --team-red-focus: {redFocus};
    --team-blue-text: {blueText};
    --team-red-text: {redText};
}}";
        }

        private string GenerateFocusColor(string hexColor)
        {
            // Parse hex color
            if (string.IsNullOrEmpty(hexColor) || !hexColor.StartsWith("#"))
                return hexColor;

            var hex = hexColor.TrimStart('#');
            if (hex.Length != 6)
                return hexColor;

            try
            {
                var r = Convert.ToInt32(hex.Substring(0, 2), 16);
                var g = Convert.ToInt32(hex.Substring(2, 2), 16);
                var b = Convert.ToInt32(hex.Substring(4, 2), 16);

                // Make color more intense by reducing it by 30% (darken)
                r = Math.Max(0, (int)(r * 0.7));
                g = Math.Max(0, (int)(g * 0.7));
                b = Math.Max(0, (int)(b * 0.7));

                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return hexColor;
            }
        }

        private string GetTextColor(string hexColor)
        {
            // Parse hex color and calculate luminance
            if (string.IsNullOrEmpty(hexColor) || !hexColor.StartsWith("#"))
                return "#000000";

            var hex = hexColor.TrimStart('#');
            if (hex.Length != 6)
                return "#000000";

            try
            {
                var r = Convert.ToInt32(hex.Substring(0, 2), 16);
                var g = Convert.ToInt32(hex.Substring(2, 2), 16);
                var b = Convert.ToInt32(hex.Substring(4, 2), 16);

                // Calculate relative luminance (ITU-R BT.709)
                var luminance = (0.2126 * r + 0.7152 * g + 0.0722 * b) / 255.0;

                // Use black text for light backgrounds, white for dark
                return luminance > 0.5 ? "#000000" : "#ffffff";
            }
            catch
            {
                return "#000000";
            }
        }
    }
}

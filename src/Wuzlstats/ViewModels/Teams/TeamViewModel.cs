using System;

namespace Wuzlstats.ViewModels.Teams
{
    public class TeamViewModel
    {
        public PlayerViewModel Player1 { get; set; }
        public PlayerViewModel Player2 { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public DateTime LastGamePlayedOn { get; set; }
        public int GamesCount { get; set; }

        // ReSharper disable once PossibleLossOfFraction
        public double Rank => Losses == 0 ? Wins : (Wins == 0 ? 0.1d / Losses : (double)Wins / Losses);

        public bool Equals(PlayerViewModel p1, PlayerViewModel p2)
        {
            if (Player1 == null || Player2 == null)
            {
                return false;
            }
            if (p1.Id <= p2.Id)
            {
                return Player1.Id == p1.Id && Player2.Id == p2.Id;
            }
            return Player2.Id == p1.Id && Player1.Id == p2.Id;
        }
    }
}

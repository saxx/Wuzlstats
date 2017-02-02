using System;

namespace Wuzlstats.ViewModels.Home
{
    public class DeleteGameViewModel
    {
        public int Id { get; set; }
        public DateTime PlayedOn { get; set; }
        public string RedPlayers { get; set; }
        public string BluePlayers { get; set; }

        public int RedScore { get; set; }
        public int BlueScore { get; set; }
    }
}

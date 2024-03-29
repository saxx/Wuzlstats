namespace Wuzlstats.Models
{
    public class PlayerPosition
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public PlayerPositionTypes Position { get; set; }

        public Game? Game { get; set; }
        public Player? Player { get; set; }

        public bool IsBluePosition => Position == PlayerPositionTypes.Blue || Position == PlayerPositionTypes.BlueDefense || Position == PlayerPositionTypes.BlueOffense;
        public bool IsRedPosition => Position == PlayerPositionTypes.Red || Position == PlayerPositionTypes.RedDefense || Position == PlayerPositionTypes.RedOffense;
    }

    public enum PlayerPositionTypes
    {
        Red,
        Blue,
        RedOffense,
        RedDefense,
        BlueOffense,
        BlueDefense
    }
}

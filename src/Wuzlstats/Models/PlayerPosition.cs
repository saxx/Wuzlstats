namespace Wuzlstats.Models
{
    public class PlayerPosition
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public PlayerPositionTypes Position { get; set; }

        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }

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
namespace Wuzlstats.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? TimeoutConfiguration { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}

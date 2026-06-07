

namespace DomainLayer.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public DateTime Date { get; set; }

        // Initialize collection to avoid null reference issues
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
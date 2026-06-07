using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.Dtos
{
    public class TournamentResponseDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        public required string Title { get; set; }
        public required string Description { get; set; }

        [Range(1, 500)]
        public int MaxPlayers { get; set; }
        // Date removed → satisfies the “remove one property” requirement

        public ICollection<GameResponseDTO> Games { get; set; } = new List<GameResponseDTO>();
    }
}
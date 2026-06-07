using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.Dtos
{
    // DTO acts like a filter for the model, controlling what data is sent to or received from the API.
    // This way, sensitive or internal fields are not exposed to the client.
    public class TournamentCreateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        public required string Title { get; set; }

        public required string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "MaxPlayer must be at least 1")]
        public int MaxPlayers { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [FutureDate(ErrorMessage = "Date cannot be in the past")]
        public DateTime Date { get; set; }
    }
}
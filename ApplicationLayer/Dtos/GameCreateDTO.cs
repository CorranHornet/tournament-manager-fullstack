using ApplicationLayer.Dtos;
using System.ComponentModel.DataAnnotations;

public class GameCreateDTO
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Time is required")]
    [FutureDate(ErrorMessage = "Time cannot be in the past")]
    public DateTime Time { get; set; }

    [Required(ErrorMessage = "TournamentId is required")]
    public int TournamentId { get; set; }
}
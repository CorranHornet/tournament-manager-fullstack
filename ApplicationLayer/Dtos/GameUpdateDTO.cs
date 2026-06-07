using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.Dtos
{
    public class GameUpdateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [FutureDate(ErrorMessage = "Time cannot be in the past")]
        public DateTime Time { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Game
{
    public class GameEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required!")]
        [MaxLength(100, ErrorMessage = "The title must be less than 100 characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The release year is required!")]
        [Range(1980, 2100, ErrorMessage = "The release year must be between 1980 and 2100!")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "The genre is required!")]
        [MaxLength(100, ErrorMessage = "The genre must be less than 100 characters!")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "The price is required!")]
        [Range(0.5, 200, ErrorMessage = "The price must be between 0.5 and 200!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The rating is required!")]
        [Range(0.5, 5, ErrorMessage = "The rating must be between 0.5 and 5!")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "The developer ID is required!")]
        public int DeveloperId { get; set; }
    }
}

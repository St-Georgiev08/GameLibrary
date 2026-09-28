using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Developer
{
    public class DeveloperCreateViewModel
    {


        [Required(ErrorMessage = "Developer name is required!")]
        [MaxLength(100, ErrorMessage = "The developer name must be less than 100 characters!")]
        public string DeveloperName { get; set; }

        [Required(ErrorMessage = "Countrt is required!")]
        [MaxLength(100, ErrorMessage = "The country must be less than 100 characters!")]
        public string Country { get; set; }

        [EmailAddress(ErrorMessage = "Wrong input!")]
        public string? Email { get; set; }

        [Range(1980, 2100, ErrorMessage = "The rating must be between 1980 and 2100!")]
        public int FoundedYear { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Platform
{
    public class CreatePlatformViewModel
    {
        [Required(ErrorMessage = "Platform name is required.")]
        [StringLength(100)]
        [Display(Name = "Platform Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Manufacturer { get; set; }

        [Range(1970, 2100, ErrorMessage = "Enter a valid release year.")]
        [Display(Name = "Release Year")]
        public int? ReleaseYear { get; set; }
    }
}

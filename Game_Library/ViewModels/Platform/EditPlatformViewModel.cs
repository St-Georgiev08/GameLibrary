using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Platform
{
    public class EditPlatformViewModel
    {
        public int PlatformId { get; set; }

        [Required(ErrorMessage = "Platform name is required.")]
        [StringLength(100)]
        [Display(Name = "Platform Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Manufacturer { get; set; }

        [Range(1970, 2100)]
        [Display(Name = "Release Year")]
        public int? ReleaseYear { get; set; }
    }
}

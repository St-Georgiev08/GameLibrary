using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Developer
{
    public class DeveloperDetailsViewModel
    {
        public string DeveloperName { get; set; }

        public string Country { get; set; }

        public string? Email { get; set; }
        public int FoundedYear { get; set; }
    }
}

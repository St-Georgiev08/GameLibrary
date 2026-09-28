using System.ComponentModel.DataAnnotations;

namespace Game_Library.ViewModels.Developer
{
    public class DeveloperIndexViewModel
    {
        public int Developer_id { get; set; }

        public string DeveloperName { get; set; }

        public string Country { get; set; }
        public string? Email { get; set; }

        public int FoundedYear { get; set; }
    }
}

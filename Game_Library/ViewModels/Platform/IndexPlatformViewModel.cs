namespace Game_Library.ViewModels.Platform
{
    public class IndexPlatformViewModel
    {
        public int PlatformId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Manufacturer { get; set; }

        public int? ReleaseYear { get; set; }

        public int GamesCount { get; set; }
    }
}

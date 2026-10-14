namespace Game_Library.ViewModels.Platform
{
    public class DetailsPlatformViewModel
    {

            public int PlatformId { get; set; }

            public string Name { get; set; } = string.Empty;

            public string? Manufacturer { get; set; }

            public int? ReleaseYear { get; set; }

            public List<PlatformGameViewModel> Games { get; set; }

        public class PlatformGameViewModel
        {
            public int GameId { get; set; }

            public string Title { get; set; } = string.Empty;

            public int ReleaseYear { get; set; }
        }
    }
}

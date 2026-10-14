using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary_Data.Entities
{
    public class Platform
    {
        [Key]
        public int PlatformId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        public int? ReleaseYear { get; set; }

        public ICollection<GamePlatform> GamePlatforms { get; set; }
            = new List<GamePlatform>();
    }

}

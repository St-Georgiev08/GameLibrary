using System.ComponentModel.DataAnnotations;

namespace GameLibrary_Data.Entities
{
    public class Developer
    {
        [Key]
        public int Developer_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeveloperName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Range(1980, 2100)]
        public int FoundedYear { get; set; }

        [EmailAddress]
        public string? Email {  get; set; }

        public bool IsDeleted { get; set; }


        public ICollection<Game> Games { get; set; }
    }
}
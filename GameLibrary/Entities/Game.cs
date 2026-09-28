using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary_Data.Entities
{
    public class Game
    {
        [Key]
        public int Game_id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(100)]
        public string Genre { get; set; }

        [Required]
        [Range(0.5,200)]
        public decimal Price { get; set; }

        [Required]
        [Range(0.5,5)]
        public decimal Reating { get; set; }


        [Required]
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }

        

    }
}

using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary_Data
{
    public class GameLibraryDbContext:DbContext
    {
        public GameLibraryDbContext(DbContextOptions<GameLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameLibrary_Data.Config
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Game_id);

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x=>x.ReleaseYear)
                .IsRequired();

            builder.Property(x => x.Genre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            builder.HasOne(x=>x.Developer)
                .WithMany(x=>x.Games)
                .HasForeignKey(x => x.DeveloperId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Game
                {
                    Game_id = 1,
                    Title = "The Legend of Zelda: Breath of the Wild",
                    ReleaseYear = 2017,
                    Genre = "Action-adventure",
                    DeveloperId = 1
                },
                new Game
                {
                    Game_id = 2,
                    Title = "Half-Life: Alyx",
                    ReleaseYear = 2020,
                    Genre = "First-person shooter",
                    DeveloperId = 2
                });
        }
    }
}

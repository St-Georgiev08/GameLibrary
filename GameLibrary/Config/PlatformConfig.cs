using GameLibrary_Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary_Data.Config
{
    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(p => p.PlatformId);

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Manufacturer)
                .HasMaxLength(100);

            builder.Property(p => p.ReleaseYear)
                .IsRequired(false);

            builder.HasData(
                new Platform
                {
                    PlatformId = 1,
                    Name = "Nintendo Switch",
                    Manufacturer = "Nintendo",
                    ReleaseYear = 2017
                },
                new Platform
                {
                    PlatformId = 2,
                    Name = "PC (Valve Index)",
                    Manufacturer = "Valve Corporation",
                    ReleaseYear = 2020
                });
        }
    }
}

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
    public class DeveloperConfig : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> builder)
        {
            builder.HasKey(d => d.Developer_id);
            builder.Property(d => d.DeveloperName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.FoundedYear)
                .IsRequired();

            builder.Property(d => d.Email)
                .HasMaxLength(300);


            builder.HasData(
                new Developer
                {
                    Developer_id = 1,
                    DeveloperName = "Nintendo",
                    Country = "Japan",
                    FoundedYear = 1900,
                    Email = "",
                    IsDeleted = false
                },
                new Developer
                {
                    Developer_id = 2,
                    DeveloperName = "Valve Corporation",
                    Country = "United States",
                    FoundedYear = 1996,
                    Email = "",
                    IsDeleted = false
                });
        }
    }
}

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
    public class GamePlatformConfig : IEntityTypeConfiguration<GamePlatform>
    {
        public void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            builder.HasData(
                new GamePlatform
                {
                    GameId = 1,
                    PlatformId = 1
                },
                new GamePlatform
                {
                    GameId = 2,
                    PlatformId = 2
                });
        }
    }
}

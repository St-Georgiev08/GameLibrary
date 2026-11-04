using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameLibrary_Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationForEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Developers",
                columns: new[] { "Developer_id", "Country", "DeveloperName", "Email", "FoundedYear", "IsDeleted" },
                values: new object[,]
                {
                    { 1, "Japan", "Nintendo", "", 1889, false },
                    { 2, "United States", "Valve Corporation", "", 1996, false }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "PlatformId", "Manufacturer", "Name", "ReleaseYear" },
                values: new object[,]
                {
                    { 1, "Nintendo", "Nintendo Switch", 2017 },
                    { 2, "Valve Corporation", "PC (Valve Index)", 2020 }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Game_id", "DeveloperId", "Genre", "Price", "Reating", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Action-adventure", 0m, 0m, 2017, "The Legend of Zelda: Breath of the Wild" },
                    { 2, 2, "First-person shooter", 0m, 0m, 2020, "Half-Life: Alyx" }
                });

            migrationBuilder.InsertData(
                table: "GamePlatforms",
                columns: new[] { "GameId", "PlatformId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GamePlatforms",
                keyColumns: new[] { "GameId", "PlatformId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "GamePlatforms",
                keyColumns: new[] { "GameId", "PlatformId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Game_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Game_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Developers",
                keyColumn: "Developer_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Developers",
                keyColumn: "Developer_id",
                keyValue: 2);
        }
    }
}

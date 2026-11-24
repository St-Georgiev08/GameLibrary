using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary_Data.Migrations
{
    /// <inheritdoc />
    public partial class configFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games");

            migrationBuilder.UpdateData(
                table: "Developers",
                keyColumn: "Developer_id",
                keyValue: 1,
                column: "FoundedYear",
                value: 1900);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Developer_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games");

            migrationBuilder.UpdateData(
                table: "Developers",
                keyColumn: "Developer_id",
                keyValue: 1,
                column: "FoundedYear",
                value: 1889);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Developer_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

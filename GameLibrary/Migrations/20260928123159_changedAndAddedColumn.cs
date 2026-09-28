using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary_Data.Migrations
{
    /// <inheritdoc />
    public partial class changedAndAddedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Reating",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Developers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Developers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Developers");

            migrationBuilder.AlterColumn<double>(
                name: "Reating",
                table: "Games",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}

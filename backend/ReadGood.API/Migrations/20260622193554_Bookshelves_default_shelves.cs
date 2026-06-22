using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadGood.API.Migrations
{
    /// <inheritdoc />
    public partial class Bookshelves_default_shelves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultShelf",
                table: "Bookshelves",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultShelf",
                table: "Bookshelves");
        }
    }
}

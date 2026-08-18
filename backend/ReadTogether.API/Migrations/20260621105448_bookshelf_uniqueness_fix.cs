using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadTogether.API.Migrations
{
    /// <inheritdoc />
    public partial class bookshelf_uniqueness_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookshelves_Name",
                table: "Bookshelves");

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelves_Name_UserId",
                table: "Bookshelves",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookshelves_Name_UserId",
                table: "Bookshelves");

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelves_Name",
                table: "Bookshelves",
                column: "Name",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReadTogether.API.Migrations
{
    /// <inheritdoc />
    public partial class addedbookshelves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookshelves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookshelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookshelves_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookshelfBooks",
                columns: table => new
                {
                    BookshelfId = table.Column<int>(type: "integer", nullable: false),
                    VolumeId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookshelfBooks", x => new { x.BookshelfId, x.VolumeId });
                    table.ForeignKey(
                        name: "FK_BookshelfBooks_Bookshelves_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "Bookshelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelves_Name",
                table: "Bookshelves",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookshelves_UserId",
                table: "Bookshelves",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookshelfBooks");

            migrationBuilder.DropTable(
                name: "Bookshelves");
        }
    }
}

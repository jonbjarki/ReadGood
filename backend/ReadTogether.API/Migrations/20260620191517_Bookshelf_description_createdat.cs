using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadTogether.API.Migrations
{
    /// <inheritdoc />
    public partial class Bookshelf_description_createdat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookshelves",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Bookshelves",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookshelves");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Bookshelves");
        }
    }
}

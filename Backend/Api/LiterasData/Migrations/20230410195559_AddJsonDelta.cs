using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiterasData.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonDelta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "ContentDeltas",
                table: "Docs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "TitleDelta",
                table: "Docs",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentDeltas",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "TitleDelta",
                table: "Docs");
        }
    }
}

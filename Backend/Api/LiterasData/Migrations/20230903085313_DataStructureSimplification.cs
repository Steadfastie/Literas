using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiterasData.Migrations
{
    /// <inheritdoc />
    public partial class DataStructureSimplification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editors_Users_UserId",
                table: "Editors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Editors_UserId",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "ContentDeltas",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Docs");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastContributed",
                table: "Editors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<List<string>>(
                name: "Scopes",
                table: "Editors",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Editors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "TitleDelta",
                table: "Docs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "ContentDelta",
                table: "Docs",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Docs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Docs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Editors_UserId_DocId",
                table: "Editors",
                columns: new[] { "UserId", "DocId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Editors_UserId_DocId",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "LastContributed",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "Scopes",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "ContentDelta",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Docs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Docs");

            migrationBuilder.AlterColumn<JsonDocument>(
                name: "TitleDelta",
                table: "Docs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(JsonDocument),
                oldType: "jsonb");

            migrationBuilder.AddColumn<JsonDocument>(
                name: "ContentDeltas",
                table: "Docs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Docs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Fullname = table.Column<string>(type: "text", nullable: true),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Editors_UserId",
                table: "Editors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editors_Users_UserId",
                table: "Editors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

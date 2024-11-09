using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mote.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReallyAddPathAndSlugToNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Notes");
        }
    }
}

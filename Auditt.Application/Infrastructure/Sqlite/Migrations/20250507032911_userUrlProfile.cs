using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class userUrlProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlProfile",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlProfile",
                table: "Users");
        }
    }
}

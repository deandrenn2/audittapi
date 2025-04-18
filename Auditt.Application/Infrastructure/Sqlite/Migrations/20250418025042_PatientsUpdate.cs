using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class PatientsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Questions",
                newName: "Text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Questions",
                newName: "Name");
        }
    }
}

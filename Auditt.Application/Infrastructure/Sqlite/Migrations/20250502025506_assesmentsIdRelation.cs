using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class assesmentsIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdDataCut",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdFunctionary",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdGuide",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdInstitution",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdPatient",
                table: "Assessments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdDataCut",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdFunctionary",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdGuide",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdInstitution",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPatient",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class EntityUserIdRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Guides_GuideId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdRol",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "IdQuestion",
                table: "Valuations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuideId",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdGuide",
                table: "Assessments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Guides_GuideId",
                table: "Assessments",
                column: "GuideId",
                principalTable: "Guides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Guides_GuideId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IdQuestion",
                table: "Valuations");

            migrationBuilder.DropColumn(
                name: "IdGuide",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GuideId",
                table: "Assessments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Guides_GuideId",
                table: "Assessments",
                column: "GuideId",
                principalTable: "Guides",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class EntityFunctionry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Functionaries_FunctionaryId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_FunctionaryId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FunctionaryId",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionaryId",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_FunctionaryId",
                table: "Roles",
                column: "FunctionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Functionaries_FunctionaryId",
                table: "Roles",
                column: "FunctionaryId",
                principalTable: "Functionaries",
                principalColumn: "Id");
        }
    }
}

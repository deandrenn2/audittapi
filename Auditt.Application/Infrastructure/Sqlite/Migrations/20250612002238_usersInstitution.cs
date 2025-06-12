using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class usersInstitution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Instituciones_InstitutionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InstitutionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Instituciones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InstitutionUser",
                columns: table => new
                {
                    InstitutionsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionUser", x => new { x.InstitutionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_InstitutionUser_Instituciones_InstitutionsId",
                        column: x => x.InstitutionsId,
                        principalTable: "Instituciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionUser_UsersId",
                table: "InstitutionUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionUser");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Instituciones");

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InstitutionId",
                table: "Users",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Instituciones_InstitutionId",
                table: "Users",
                column: "InstitutionId",
                principalTable: "Instituciones",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class configIndexPatientAndFunctionay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Functionaries_FunctionaryId",
                table: "Assessments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Functionaries",
                table: "Functionaries");

            migrationBuilder.RenameTable(
                name: "Functionaries",
                newName: "Funcionarios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Funcionarios",
                table: "Funcionarios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Identification",
                table: "Patients",
                column: "Identification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Identification",
                table: "Funcionarios",
                column: "Identification",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Funcionarios_FunctionaryId",
                table: "Assessments",
                column: "FunctionaryId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Funcionarios_FunctionaryId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Identification",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Funcionarios",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_Identification",
                table: "Funcionarios");

            migrationBuilder.RenameTable(
                name: "Funcionarios",
                newName: "Functionaries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Functionaries",
                table: "Functionaries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Functionaries_FunctionaryId",
                table: "Assessments",
                column: "FunctionaryId",
                principalTable: "Functionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

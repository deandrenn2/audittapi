using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class nitUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Institutions_InstitutionId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_DataCuts_Institutions_InstitutionId",
                table: "DataCuts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Institutions_InstitutionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions");

            migrationBuilder.RenameTable(
                name: "Institutions",
                newName: "Instituciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instituciones",
                table: "Instituciones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Instituciones_Nit",
                table: "Instituciones",
                column: "Nit",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Instituciones_InstitutionId",
                table: "Assessments",
                column: "InstitutionId",
                principalTable: "Instituciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataCuts_Instituciones_InstitutionId",
                table: "DataCuts",
                column: "InstitutionId",
                principalTable: "Instituciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Instituciones_InstitutionId",
                table: "Users",
                column: "InstitutionId",
                principalTable: "Instituciones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Instituciones_InstitutionId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_DataCuts_Instituciones_InstitutionId",
                table: "DataCuts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Instituciones_InstitutionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instituciones",
                table: "Instituciones");

            migrationBuilder.DropIndex(
                name: "IX_Instituciones_Nit",
                table: "Instituciones");

            migrationBuilder.RenameTable(
                name: "Instituciones",
                newName: "Institutions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Institutions_InstitutionId",
                table: "Assessments",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataCuts_Institutions_InstitutionId",
                table: "DataCuts",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Institutions_InstitutionId",
                table: "Users",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id");
        }
    }
}

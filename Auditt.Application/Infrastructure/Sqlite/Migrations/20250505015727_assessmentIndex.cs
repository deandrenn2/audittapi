using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auditt.Application.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class assessmentIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_InstitutionId",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_InstitutionId_DataCutId_FunctionaryId_PatientId_GuideId",
                table: "Assessments",
                columns: new[] { "InstitutionId", "DataCutId", "FunctionaryId", "PatientId", "GuideId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_InstitutionId_DataCutId_FunctionaryId_PatientId_GuideId",
                table: "Assessments");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_InstitutionId",
                table: "Assessments",
                column: "InstitutionId");
        }
    }
}

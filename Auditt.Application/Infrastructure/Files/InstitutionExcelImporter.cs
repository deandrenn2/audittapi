using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using ClosedXML.Excel;

namespace Auditt.Application.Infrastructure.Files;

public class InstitutionExcelImporter : ExcelImporter<Institution>
{
    protected override string[] ColumnHeaders => new[]
    {
        "Name",
        "Abbreviation",
        "Nit",
        "City",
        "Manager",
        "AssistantManager"
    };

    protected override Institution? CreateEntityFromRow(IXLRow row)
    {
        var name = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(name)) return null; // Skip empty rows

        return Institution.Create(
            id: 0, // ID will be assigned by the database
            name: row.Cell(1).GetString(),
            abbreviation: row.Cell(2).GetString(),
            nit: row.Cell(3).GetString(),
            city: row.Cell(4).GetString(),
            manager: row.Cell(5).GetString(),
            assistantManager: row.Cell(6).GetString()
        );
    }

    protected override bool EntityExists(Institution entity, AppDbContext dbContext)
    {
        // Check if an institution with the same Nit already exists
        return dbContext.Institutions.Any(i => i.Nit == entity.Nit);
    }

    protected override void FormatTemplate(IXLWorksheet worksheet)
    {
        // We can add specific formatting for Institution template here
        // For example, setting column widths
        worksheet.Column(1).Width = 30; // Name
        worksheet.Column(2).Width = 15; // Abbreviation
        worksheet.Column(3).Width = 15; // Nit
        worksheet.Column(4).Width = 20; // City
        worksheet.Column(5).Width = 25; // Manager
        worksheet.Column(6).Width = 25; // AssistantManager
    }
}

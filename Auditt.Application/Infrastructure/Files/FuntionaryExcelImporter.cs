using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using ClosedXML.Excel;

namespace Auditt.Application.Infrastructure.Files;

public class FunctionaryExcelImporter : ExcelImporter<Functionary>
{
    protected override string[] ColumnHeaders => new[]
    {
        "FirstName",
        "LastName",
        "Identification",
    };

    protected override Functionary? CreateEntityFromRow(IXLRow row)
    {
        var firstName = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(firstName)) return null; // Skip empty rows

        return Functionary.Create(
            id: 0, // ID will be assigned by the database
            firstName: firstName,
            lastName: row.Cell(2).GetString(),
            identification: row.Cell(3).GetString()
        );
    }

    protected override bool EntityExists(Functionary entity, AppDbContext dbContext)
    {
        // Check if a functionary with the same Identification already exists
        return dbContext.Functionaries.Any(f => f.Identification == entity.Identification);
    }

    protected override void FormatTemplate(IXLWorksheet worksheet)
    {
        // We can add specific formatting for Functionary template here
        // For example, setting column widths
        worksheet.Column(1).Width = 20; // FirstName
        worksheet.Column(2).Width = 20; // LastName
        worksheet.Column(3).Width = 15; // Identification
    }
}
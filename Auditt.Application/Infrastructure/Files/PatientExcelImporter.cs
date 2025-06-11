using Auditt.Application.Domain.Entities;
using ClosedXML.Excel;

namespace Auditt.Application.Infrastructure.Files;

public class PatientExcelImporter : ExcelImporter<Patient>
{
    protected override string[] ColumnHeaders => new[]
    {
        "FirstName",
        "LastName",
        "Identification",
        "BirthDate",
        "Eps"
    };

    protected override Patient CreateEntityFromRow(IXLRow row)
    {
        var firstName = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(firstName)) return null; // Skip empty rows

        return Patient.Create(
            id: 0, // ID will be assigned by the database
            firstName: row.Cell(1).GetString(),
            lastName: row.Cell(2).GetString(),
            identification: row.Cell(3).GetString(),
            birthDate: row.Cell(4).GetDateTime(),
            eps: row.Cell(5).GetString()
        );
    }
    protected override void FormatTemplate(IXLWorksheet worksheet)
    {
        // We can add specific formatting for Patient template here
        // For example, setting column widths
        worksheet.Column(1).Width = 20; // FirstName
        worksheet.Column(2).Width = 20; // LastName
        worksheet.Column(3).Width = 15; // Identification
        worksheet.Column(4).Width = 20; // BirthDate
        worksheet.Column(5).Width = 15; // Eps
    }

}
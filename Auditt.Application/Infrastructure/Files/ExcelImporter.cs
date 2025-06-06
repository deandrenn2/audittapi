using System.Data;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Auditt.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Auditt.Application.Infrastructure.Sqlite;

namespace Auditt.Application.Infrastructure.Files;

public abstract class ExcelImporter<T> : IExcelImporter<T>
{
    protected abstract string[] ColumnHeaders { get; }
    protected abstract T CreateEntityFromRow(IXLRow row);
    protected abstract void FormatTemplate(IXLWorksheet worksheet);

    public async Task<Result> ImportFromExcelAsync(IFormFile file, AppDbContext dbContext)
    {
        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);
            var rowCount = worksheet.RowsUsed().Count();

            var entities = new List<T>();

            for (int row = 2; row <= rowCount; row++) // Start from 2 to skip header
            {
                try
                {
                    var entity = CreateEntityFromRow(worksheet.Row(row));
                    if (entity != null)
                    {
                        dbContext.Add(entity);
                        entities.Add(entity);
                    }
                }
                catch (Exception ex)
                {
                    return Result.Failure(new Error("Import.Error", $"Row {row}: {ex.Message}"));
                }
            }

            if (entities.Count > 0)
            {
                await dbContext.SaveChangesAsync();
            }
            else
            {
                return Result.Failure(new Error("Import.Error", "No valid data found to import."));
            }

            return Result<List<T>>.Success(entities.ToList(), "Datos importados correctamente.");
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Import.Error", $"Se produjo un error al importar el archivo: {ex.Message}"));
        }
    }

    public IXLWorkbook CreateTemplate()
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Data");

        // Add headers
        for (int i = 0; i < ColumnHeaders.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = ColumnHeaders[i];
        }

        // Format headers
        var headerRange = worksheet.Range(1, 1, 1, ColumnHeaders.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Apply additional formatting
        FormatTemplate(worksheet);

        return workbook;
    }
}

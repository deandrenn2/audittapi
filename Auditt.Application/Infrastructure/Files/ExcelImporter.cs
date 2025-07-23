using System.Data;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Auditt.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Auditt.Application.Infrastructure.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Infrastructure.Files;

public abstract class ExcelImporter<T> : IExcelImporter<T>
{
    protected abstract string[] ColumnHeaders { get; }
    protected abstract T? CreateEntityFromRow(IXLRow row);
    protected abstract void FormatTemplate(IXLWorksheet worksheet);
    protected abstract bool EntityExists(T entity, AppDbContext dbContext);

    public async Task<Result> ImportFromExcelAsync(IFormFile file, AppDbContext dbContext)
    {
        var detailedResult = await ImportFromExcelDetailedAsync(file, dbContext);

        if (detailedResult.ValidationErrors.Any() && detailedResult.SuccessfulEntities.Count == 0)
        {
            return Result.Failure(new Error("Import.Error", detailedResult.Summary));
        }

        return Result<List<T>>.Success(detailedResult.SuccessfulEntities, detailedResult.Summary);
    }

    public async Task<ImportResult<T>> ImportFromExcelDetailedAsync(IFormFile file, AppDbContext dbContext)
    {
        var entities = new List<T>();
        var duplicateRows = new List<string>();
        var errorRows = new List<string>();
        int totalProcessed = 0;

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);
            var rowCount = worksheet.RowsUsed().Count();

            for (int row = 2; row <= rowCount; row++) // Start from 2 to skip header
            {
                totalProcessed++;
                try
                {
                    var entity = CreateEntityFromRow(worksheet.Row(row));
                    if (entity != null)
                    {
                        // Verificar si la entidad ya existe en la base de datos
                        if (EntityExists(entity, dbContext))
                        {
                            duplicateRows.Add($"Row {row}: Entity already exists with unique constraint violation");
                            continue;
                        }

                        dbContext.Add(entity);
                        entities.Add(entity);
                    }
                    else
                    {
                        errorRows.Add($"Row {row}: Empty or invalid row data");
                    }
                }
                catch (Exception ex)
                {
                    errorRows.Add($"Row {row}: {ex.Message}");
                }
            }

            if (entities.Count > 0)
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Manejar violaciones de restricciones únicas que no se detectaron antes
                    errorRows.Add($"Database constraint violation: {ex.InnerException?.Message ?? ex.Message}");
                    entities.Clear(); // Limpiar entidades si falla el guardado
                }
            }

            var summary = $"Importación terminada. Correctamente importados: {entities.Count}";
            if (duplicateRows.Count > 0)
            {
                summary += $", Duplicados omitidos: {duplicateRows.Count}";
            }
            if (errorRows.Count > 0)
            {
                summary += $", Errores: {errorRows.Count}";
            }

            return new ImportResult<T>(entities, duplicateRows, errorRows, totalProcessed, summary);
        }
        catch (Exception ex)
        {
            errorRows.Add($"Error general: {ex.Message}");
            return new ImportResult<T>(entities, duplicateRows, errorRows, totalProcessed, $"Importación fallida: {ex.Message}");
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

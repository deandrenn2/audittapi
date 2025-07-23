using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace Auditt.Application.Infrastructure.Files;

public record ImportResult<T>(
    List<T> SuccessfulEntities,
    List<string> DuplicateErrors,
    List<string> ValidationErrors,
    int TotalProcessed,
    string Summary
);

public interface IExcelImporter<T>
{
    Task<Result> ImportFromExcelAsync(IFormFile file, AppDbContext dbContext);
    Task<ImportResult<T>> ImportFromExcelDetailedAsync(IFormFile file, AppDbContext dbContext);
    IXLWorkbook CreateTemplate();
}

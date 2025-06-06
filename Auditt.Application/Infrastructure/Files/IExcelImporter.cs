using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace Auditt.Application.Infrastructure.Files;

public interface IExcelImporter<T>
{
    Task<Result> ImportFromExcelAsync(IFormFile file, AppDbContext dbContext);
    IXLWorkbook CreateTemplate();
}

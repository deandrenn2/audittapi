using Microsoft.AspNetCore.Http;
using Auditt.Domain.Shared;

namespace Auditt.Application.Infrastructure.Files;
public interface IFileManager
{
    Task<Result> UploadFileAsync(IFormFile file);
}
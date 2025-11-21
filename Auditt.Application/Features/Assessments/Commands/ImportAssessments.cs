using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using System.Security.Claims;

namespace Auditt.Application.Features.Assessments;

/// <summary>
/// Endpoint para importar evaluaciones desde un archivo Excel
/// </summary>
public class ImportAssessments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/assessments/import", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];

            if (file == null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "No se proporcionó ningún archivo para importar." } }
                });
            }

            // Obtener el ID del usuario autenticado
            var userId = 0;
            var userIdClaim = req.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            // Obtener los parámetros de contexto desde el formulario
            if (!int.TryParse(form["institutionId"], out var institutionId) ||
                !int.TryParse(form["dataCutId"], out var dataCutId) ||
                !int.TryParse(form["guideId"], out var guideId))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "Context", new[] { "Debe proporcionar institutionId, dataCutId y guideId." } }
                });
            }

            return await mediator.Send(new ImportAssessmentsCommand(file, userId, institutionId, dataCutId, guideId));
        })
        .WithName(nameof(ImportAssessments))
        .WithTags(nameof(Assessment))
        .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportAssessmentsResponse>(StatusCodes.Status200OK)
        .RequireAuthorization();
    }

    public record ImportAssessmentsCommand(IFormFile File, int UserId, int InstitutionId, int DataCutId, int GuideId) : IRequest<IResult>;

    public record ImportAssessmentsResponse(
        int SuccessCount,
        int DuplicateCount,
        int ErrorCount,
        int TotalProcessed,
        string Summary,
        List<string> Duplicates,
        List<string> Errors
    );

    public class ImportAssessmentsHandler(
        AssessmentExcelImporter importer,
        AppDbContext dbContext) : IRequestHandler<ImportAssessmentsCommand, IResult>
    {
        public async Task<IResult> Handle(ImportAssessmentsCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "No se proporcionó ningún archivo para importar." } }
                });
            }

            if (request.UserId <= 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "UserId", new[] { "Usuario no autenticado o inválido." } }
                });
            }

            // Validar extensión del archivo
            var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "El archivo debe ser un documento Excel (.xlsx o .xls)." } }
                });
            }

            try
            {
                var result = await importer.ImportAssessmentsWithContextAsync(
                    request.File,
                    dbContext,
                    request.UserId,
                    request.InstitutionId,
                    request.DataCutId,
                    request.GuideId);

                var response = new ImportAssessmentsResponse(
                    SuccessCount: result.SuccessfulEntities.Count,
                    DuplicateCount: result.DuplicateErrors.Count,
                    ErrorCount: result.ValidationErrors.Count,
                    TotalProcessed: result.TotalProcessed,
                    Summary: result.Summary,
                    Duplicates: result.DuplicateErrors,
                    Errors: result.ValidationErrors
                );

                // Si hay evaluaciones importadas, retornar éxito con detalles
                if (result.SuccessfulEntities.Count > 0)
                {
                    return Results.Ok(Result<ImportAssessmentsResponse>.Success(
                        response,
                        result.Summary
                    ));
                }

                // Si no hay éxitos pero hay errores, retornar con mensaje de fallo
                if (result.ValidationErrors.Count > 0 || result.DuplicateErrors.Count > 0)
                {
                    return Results.Ok(Result<ImportAssessmentsResponse>.Success(
                        response,
                        "Importación completada con errores. Revise los detalles."
                    ));
                }

                // Si no hay datos procesados
                return Results.Ok(Result.Failure(
                    new Error("Import.NoData", "No se encontraron datos válidos para importar.")
                ));
            }
            catch (Exception ex)
            {
                return Results.Ok(Result.Failure(
                    new Error("Import.Error", $"Error al importar evaluaciones: {ex.Message}")
                ));
            }
        }
    }

    public class ImportAssessmentsValidator : AbstractValidator<ImportAssessmentsCommand>
    {
        public ImportAssessmentsValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("Debe proporcionar un archivo para importar.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es requerido.");

            RuleFor(x => x.InstitutionId)
                .GreaterThan(0).WithMessage("El ID de institución es requerido.");

            RuleFor(x => x.DataCutId)
                .GreaterThan(0).WithMessage("El ID de corte de datos es requerido.");

            RuleFor(x => x.GuideId)
                .GreaterThan(0).WithMessage("El ID de guía es requerido.");
        }
    }
}

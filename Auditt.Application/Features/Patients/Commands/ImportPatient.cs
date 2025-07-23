using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;
using Auditt.Application.Domain.Models;
using Auditt.Application.Infrastructure.Sqlite;

namespace Auditt.Application.Features.Patients;

public class ImportPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/patients/import", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];
            return await mediator.Send(new ImportPatientCommand(file));
        })
        .WithName(nameof(ImportPatient))
        .WithTags(nameof(Patient))
          .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportPatientResponse>(StatusCodes.Status200OK);

        app.MapPost("api/patients/import-detailed", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];
            return await mediator.Send(new ImportPatientDetailedCommand(file));
        })
        .WithName("ImportPatientDetailed")
        .WithTags(nameof(Patient))
          .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportPatientDetailedResponse>(StatusCodes.Status200OK);
    }

    public record ImportPatientCommand(IFormFile File) : IRequest<IResult>;
    public record ImportPatientDetailedCommand(IFormFile File) : IRequest<IResult>;

    public record ImportPatientResponse(string Message);
    public record ImportPatientDetailedResponse(
        int SuccessfulCount,
        int DuplicatesCount,
        int ErrorsCount,
        int TotalProcessed,
        string Summary,
        List<string> DuplicateErrors,
        List<string> ValidationErrors
    );

    public class ImportPatientHandler(IExcelImporter<Patient> importer, AppDbContext dbContext) : IRequestHandler<ImportPatientCommand, IResult>
    {
        public async Task<IResult> Handle(ImportPatientCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "No file was provided for import." } }
                });
            }

            var result = await importer.ImportFromExcelAsync(request.File, dbContext);

            if (result.IsFailure)
            {
                return Results.Ok(result.Error);
            }

            return Results.Ok(new ImportPatientResponse(result?.Message ?? "Pacientes importados correctamente."));
        }
    }

    public class ImportPatientDetailedHandler(IExcelImporter<Patient> importer, AppDbContext dbContext) : IRequestHandler<ImportPatientDetailedCommand, IResult>
    {
        public async Task<IResult> Handle(ImportPatientDetailedCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "No file was provided for import." } }
                });
            }

            var result = await importer.ImportFromExcelDetailedAsync(request.File, dbContext);

            var response = new ImportPatientDetailedResponse(
                result.SuccessfulEntities.Count,
                result.DuplicateErrors.Count,
                result.ValidationErrors.Count,
                result.TotalProcessed,
                result.Summary,
                result.DuplicateErrors,
                result.ValidationErrors
            );

            return Results.Ok(response);
        }
    }

    public class ImportPatientValidator : AbstractValidator<ImportPatientCommand>
    {
        public ImportPatientValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("El archivo no puede ser nulo.");
        }
    }

    public class ImportPatientDetailedValidator : AbstractValidator<ImportPatientDetailedCommand>
    {
        public ImportPatientDetailedValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("El archivo no puede ser nulo.");
        }
    }
}
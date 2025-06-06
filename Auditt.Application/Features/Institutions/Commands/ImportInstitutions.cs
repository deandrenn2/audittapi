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

namespace Auditt.Application.Features.Institutions;

public class ImportInstitutions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/institutions/import", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];
            return await mediator.Send(new ImportInstitutionsCommand(file));
        })
        .WithName(nameof(ImportInstitutions))
        .WithTags(nameof(Institution))
          .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportInstitutionsResponse>(StatusCodes.Status200OK);
    }

    public record ImportInstitutionsCommand(IFormFile File) : IRequest<IResult>;

    public record ImportInstitutionsResponse(string Message);

    public class ImportInstitutionsHandler(IExcelImporter<Institution> importer, AppDbContext dbContext) : IRequestHandler<ImportInstitutionsCommand, IResult>
    {
        public async Task<IResult> Handle(ImportInstitutionsCommand request, CancellationToken cancellationToken)
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

            return Results.Ok(new ImportInstitutionsResponse(result?.Message ?? "Instituciones importadas correctamente."));
        }
    }

    public class ImportInstitutionsValidator : AbstractValidator<ImportInstitutionsCommand>
    {
        public ImportInstitutionsValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("A file must be provided for import.");
        }
    }
}

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

namespace Auditt.Application.Features.Functionaries;

public class ImportFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/functionaries/import", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];
            return await mediator.Send(new ImportFunctionaryCommand(file));
        })
        .WithName(nameof(ImportFunctionary))
        .WithTags(nameof(Functionary))
          .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportFunctionaryResponse>(StatusCodes.Status200OK);
    }

    public record ImportFunctionaryCommand(IFormFile File) : IRequest<IResult>;

    public record ImportFunctionaryResponse(string Message);

    public class ImportFunctionaryHandler(IExcelImporter<Functionary> importer, AppDbContext dbContext) : IRequestHandler<ImportFunctionaryCommand, IResult>
    {
        public async Task<IResult> Handle(ImportFunctionaryCommand request, CancellationToken cancellationToken)
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

            return Results.Ok(new ImportFunctionaryResponse(result?.Message ?? "Funcionarios importados correctamente."));
        }
    }

    public class ImportFunctionaryValidator : AbstractValidator<ImportFunctionaryCommand>
    {
        public ImportFunctionaryValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("El archivo no puede ser nulo.");
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("El archivo no puede estar vacío.");
        }
    }
}
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;
using Auditt.Application.Infrastructure.Sqlite;

namespace Auditt.Application.Features.Guides;

public class ImportGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/guides/import", async (HttpRequest req, IMediator mediator) =>
        {
            var form = await req.ReadFormAsync();
            var file = form.Files["file"];
            return await mediator.Send(new ImportGuideCommand(file!));
        })
        .WithName(nameof(ImportGuide))
        .WithTags(nameof(Guide))
        .Accepts<IFormFile>("multipart/form-data")
        .ProducesValidationProblem()
        .Produces<ImportGuideResponse>(StatusCodes.Status200OK);
    }

    public record ImportGuideCommand(IFormFile File) : IRequest<IResult>;

    public record ImportGuideResponse(
        bool Success,
        string Message,
        string? GuideName,
        int QuestionsCount,
        List<string> Errors
    );

    public class ImportGuideHandler(GuideExcelImporter importer, AppDbContext dbContext) : IRequestHandler<ImportGuideCommand, IResult>
    {
        public async Task<IResult> Handle(ImportGuideCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "File", new[] { "No file was provided for import." } }
                });
            }

            var result = await importer.ImportSingleGuideWithQuestionsAsync(request.File, dbContext);

            var success = result.SuccessfulEntities.Any();
            var guideName = success ? result.SuccessfulEntities.First().Name : null;
            var questionsCount = success ? await dbContext.Questions
                .CountAsync(q => q.GuideId == result.SuccessfulEntities.First().Id, cancellationToken) : 0;

            var allErrors = new List<string>();
            allErrors.AddRange(result.DuplicateErrors);
            allErrors.AddRange(result.ValidationErrors);

            var response = new ImportGuideResponse(
                success,
                result.Summary,
                guideName,
                questionsCount,
                allErrors
            );

            if (success)
            {
                return Results.Ok(response);
            }
            else
            {
                return Results.BadRequest(response);
            }
        }
    }

    public class ImportGuideValidator : AbstractValidator<ImportGuideCommand>
    {
        public ImportGuideValidator()
        {
            RuleFor(x => x.File).NotNull().WithMessage("El archivo no puede ser nulo.");
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("El archivo no puede estar vacío.");
        }
    }
}

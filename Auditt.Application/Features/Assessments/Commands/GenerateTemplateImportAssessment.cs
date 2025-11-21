using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;
using Auditt.Application.Infrastructure.Sqlite;

namespace Auditt.Application.Features.Assessments;

/// <summary>
/// Endpoint para generar la plantilla Excel de importación de evaluaciones
/// </summary>
public class GenerateTemplateImportAssessment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/assessments/template-import", async (IMediator mediator) =>
        {
            return await mediator.Send(new GenerateTemplateImportAssessmentCommand());
        })
        .WithName(nameof(GenerateTemplateImportAssessment))
        .WithTags(nameof(Assessment))
        .Produces<GenerateTemplateImportAssessmentResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportAssessmentResponse(byte[] Template, string FileName);
    public record GenerateTemplateImportAssessmentCommand : IRequest<IResult>;

    public class GenerateTemplateImportAssessmentHandler(AssessmentExcelImporter importer, AppDbContext dbContext) : IRequestHandler<GenerateTemplateImportAssessmentCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportAssessmentCommand request, CancellationToken cancellationToken)
        {
            var template = importer.CreateTemplateWithData(dbContext);

            using var stream = new MemoryStream();
            template.SaveAs(stream);
            await stream.FlushAsync(cancellationToken);
            var data = stream.ToArray();

            return Results.File(
                data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "plantilla_importacion_evaluaciones.xlsx"
            );
        }
    }
}

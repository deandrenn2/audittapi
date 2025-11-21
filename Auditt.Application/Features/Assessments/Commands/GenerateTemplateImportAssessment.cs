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
        app.MapPost("api/assessments/template-import", async (IMediator mediator, int institutionId, int dataCutId, int guideId) =>
        {
            var command = new GenerateTemplateImportAssessmentCommand(institutionId, dataCutId, guideId);
            return await mediator.Send(command);
        })
        .WithName(nameof(GenerateTemplateImportAssessment))
        .WithTags(nameof(Assessment))
        .Produces<GenerateTemplateImportAssessmentResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportAssessmentResponse(byte[] Template, string FileName);
    public record GenerateTemplateImportAssessmentCommand(int InstitutionId, int DataCutId, int GuideId) : IRequest<IResult>;

    public class GenerateTemplateImportAssessmentHandler(AssessmentExcelImporter importer, AppDbContext dbContext) : IRequestHandler<GenerateTemplateImportAssessmentCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportAssessmentCommand request, CancellationToken cancellationToken)
        {
            var template = await importer.CreateTemplateWithContextAsync(
                dbContext,
                request.InstitutionId,
                request.DataCutId,
                request.GuideId);

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

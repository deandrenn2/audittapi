using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;

namespace Auditt.Application.Features.Guides;

public class GenerateTemplateImportGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/guides/template-import", async (IMediator mediator) =>
        {
            return await mediator.Send(new GenerateTemplateImportGuideCommand());
        })
        .WithName(nameof(GenerateTemplateImportGuide))
        .WithTags(nameof(Guide))
        .Produces<GenerateTemplateImportGuideResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportGuideCommand : IRequest<IResult>;

    public record GenerateTemplateImportGuideResponse(byte[] Template, string FileName);

    public class GenerateTemplateImportGuideHandler() : IRequestHandler<GenerateTemplateImportGuideCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportGuideCommand request, CancellationToken cancellationToken)
        {
            var importer = new GuideExcelImporter();
            var template = importer.CreateTemplate();
            using var stream = new MemoryStream();
            template.SaveAs(stream);
            await stream.FlushAsync(cancellationToken);
            var data = stream.ToArray();
            return Results.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template_guides.xlsx");
        }
    }
}

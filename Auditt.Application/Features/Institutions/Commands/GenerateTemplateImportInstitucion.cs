using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;

namespace Auditt.Application.Features.Institutions;

public class GenerateTemplateImportInstitucion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/institutions/template-import", async (IMediator mediator) =>
        {
            return await mediator.Send(new GenerateTemplateImportInstitucionCommand());
        })
        .WithName(nameof(GenerateTemplateImportInstitucion))
        .WithTags(nameof(Institution))
        .Produces<GenerateTemplateImportInstitucionResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportInstitucionCommand : IRequest<IResult>;

    public record GenerateTemplateImportInstitucionResponse(byte[] TemplateFile);

    public class GenerateTemplateImportInstitucionHandler() : IRequestHandler<GenerateTemplateImportInstitucionCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportInstitucionCommand request, CancellationToken cancellationToken)
        {
            var importer = new InstitutionExcelImporter();
            var template = importer.CreateTemplate();
            using var stream = new MemoryStream();
            template.SaveAs(stream);
            await stream.FlushAsync(cancellationToken);
            var data = stream.ToArray();
            return Results.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");
        }
    }
}
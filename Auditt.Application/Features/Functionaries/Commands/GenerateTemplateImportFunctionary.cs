using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;

namespace Auditt.Application.Features.Patients;

public class GenerateTemplateImportFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/functionaries/template-import", async (IMediator mediator) =>
        {
            return await mediator.Send(new GenerateTemplateImportFunctionaryCommand());
        })
        .WithName(nameof(GenerateTemplateImportFunctionary))
        .WithTags(nameof(Functionary))
        .Produces<GenerateTemplateImportFunctionaryResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportFunctionaryCommand : IRequest<IResult>;

    public record GenerateTemplateImportFunctionaryResponse(byte[] TemplateFile);

    public class GenerateTemplateImportFunctionaryHandler() : IRequestHandler<GenerateTemplateImportFunctionaryCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportFunctionaryCommand request, CancellationToken cancellationToken)
        {
            var importer = new FunctionaryExcelImporter();
            var template = importer.CreateTemplate();
            using var stream = new MemoryStream();
            template.SaveAs(stream);
            await stream.FlushAsync(cancellationToken);
            var data = stream.ToArray();
            return Results.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");
        }
    }
}
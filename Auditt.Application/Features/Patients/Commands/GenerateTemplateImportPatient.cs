using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Files;

namespace Auditt.Application.Features.Patients;

public class GenerateTemplateImportPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/patients/template-import", async (IMediator mediator) =>
        {
            return await mediator.Send(new GenerateTemplateImportPatientCommand());
        })
        .WithName(nameof(GenerateTemplateImportPatient))
        .WithTags(nameof(Patient))
        .Produces<GenerateTemplateImportPatientResponse>(StatusCodes.Status200OK);
    }

    public record GenerateTemplateImportPatientCommand : IRequest<IResult>;

    public record GenerateTemplateImportPatientResponse(byte[] TemplateFile);

    public class GenerateTemplateImportPatientHandler() : IRequestHandler<GenerateTemplateImportPatientCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateTemplateImportPatientCommand request, CancellationToken cancellationToken)
        {
            var importer = new PatientExcelImporter();
            var template = importer.CreateTemplate();
            using var stream = new MemoryStream();
            template.SaveAs(stream);
            await stream.FlushAsync(cancellationToken);
            var data = stream.ToArray();
            return Results.File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");
        }
    }
}
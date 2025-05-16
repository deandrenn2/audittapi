using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Auditt.Application.Features.Reports;

public class GetReportGlobal : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/reports/{idDataCut:int}", async (HttpRequest req, IMediator mediator, int idDataCut, int idInstitution, int idGuide) =>
        {
            return await mediator.Send(new GetReportGlobalQuery(idDataCut, idInstitution, idGuide));
        })
        .WithName(nameof(GetReportGlobal))
        .WithTags("Report")
        .ProducesValidationProblem()
        .Produces<GetReportGlobalResponse>(StatusCodes.Status200OK);
    }
    public record GetReportGlobalQuery(int IdDataCut, int IdInstitution, int IdGuide) : IRequest<IResult>;
    public record GetReportGlobalResponse
    {
        public int CountHistories { get; set; }
        public int CountHistoriesStrictAdherence { get; set; }
        public int GlobalAdherence { get; set; }
        public int StrictAdherence
        {
            get
            {
                double value = (double)CountHistoriesStrictAdherence / CountHistories;
                int percent = (int)Math.Round(value * 100);
                return percent;
            }
        }
    };
    public class GetReportGlobalHandler(AppDbContext context, IValidator<GetReportGlobalQuery> validator) : IRequestHandler<GetReportGlobalQuery, IResult>
    {
        public async Task<IResult> Handle(GetReportGlobalQuery request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Assessment.ErrorValidation", "Se presentaron errores de validación")));
            }

            var assessments = await context.Assessments
                .Include(x => x.Valuations)
                .Include(x => x.Guide)
                .Include(x => x.Patient)
                .Where(x =>
             x.DataCutId == request.IdDataCut
            && x.InstitutionId == request.IdInstitution
            && x.GuideId == request.IdGuide
            ).ToListAsync(cancellationToken);

            var idScale = assessments.First().Guide.ScaleId;

            var scale = await context.Scales.Include(x => x.Equivalences).FirstOrDefaultAsync(x => x.Id == idScale, cancellationToken);
            var idSuccess = scale?.Equivalences.Find(x => x.Name == "Cumple")?.Id ?? 0;
            var idNoApply = scale?.Equivalences.Find(x => x.Name == "No Aplica")?.Id ?? 0;
            var idNoSuccess = scale?.Equivalences.Find(x => x.Name == "No Cumple")?.Id ?? 0;


            var AdherencePatients = assessments.SelectMany(z => z.Valuations).GroupBy(x => new
            {
                x.Assessment.PatientId,

            }).Select(d => new
            {
                CountSuccess = d.Count(x => x.EquivalenceId == idSuccess),
                CountNoApply = d.Count(x => x.EquivalenceId == idNoApply),
                CountNoSuccess = d.Count(x => x.EquivalenceId == idNoSuccess),
                ValuationsCount = d.Count(),
                d.Key.PatientId
            }).ToList();

            var AdherencePatientsPercent = AdherencePatients.Select(x => new
            {
                x.CountNoApply,
                x.CountSuccess,
                x.CountNoSuccess,
                x.ValuationsCount,
                x.PatientId,
                percentSuccess = x.CountNoSuccess + x.CountSuccess == 0 ? 0 : x.CountSuccess * 100 / (x.CountNoSuccess + x.CountSuccess),
            }).ToList();

            var AdherenceGlobal = new GetReportGlobalResponse
            {
                CountHistories = AdherencePatientsPercent.Count,
                CountHistoriesStrictAdherence = AdherencePatientsPercent.Count(x => x.percentSuccess == 100),
                GlobalAdherence = AdherencePatientsPercent.Sum(x => x.percentSuccess) / AdherencePatientsPercent.Count,
            };


            return Results.Ok(Result<GetReportGlobalResponse>.Success(AdherenceGlobal, "Se obtuvo la evaluación correctamente"));
        }
    }
    public class GetReportGlobalValidator : AbstractValidator<GetReportGlobalQuery>
    {
        public GetReportGlobalValidator()
        {
            RuleFor(x => x.IdDataCut).NotEmpty().WithMessage("El id del corte es requerido");
            RuleFor(x => x.IdInstitution).NotEmpty().WithMessage("El id de la institución es requerido");
            RuleFor(x => x.IdGuide).NotEmpty().WithMessage("El id de la guia es requerido");
        }
    }
}
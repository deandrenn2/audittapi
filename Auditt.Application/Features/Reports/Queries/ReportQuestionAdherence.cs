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
public class ReportQuestionAdherence : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/reports/question/{idDataCut:int}", async (HttpRequest req, IMediator mediator, int idDataCut, int idInstitution, int idGuide) =>
        {
            return await mediator.Send(new ReportQuestionAdherenceQuery(idDataCut, idInstitution, idGuide));
        })
        .WithName(nameof(ReportQuestionAdherence))
        .WithTags("Report")
        .ProducesValidationProblem()
        .Produces<ReportQuestionAdherenceResponse>(StatusCodes.Status200OK);
    }
    public record ReportQuestionAdherenceQuery(int IdDataCut, int IdInstitution, int IdGuide) : IRequest<IResult>;
    public record ReportQuestionAdherenceResponse(int CountSuccess, int CountNoApply, int CountNoSuccess, int ValorationsCount, int? IdQuestion, string Text, int PercentSuccess);
    public class ReportQuestionAdherenceHandler(AppDbContext context, IValidator<ReportQuestionAdherenceQuery> validator) : IRequestHandler<ReportQuestionAdherenceQuery, IResult>
    {
        public async Task<IResult> Handle(ReportQuestionAdherenceQuery request, CancellationToken cancellationToken)
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
                x.IdQuestion,
                x.Text
            }).Select(d => new
            {
                CountSuccess = d.Count(x => x.EquivalenceId == idSuccess),
                CountNoApply = d.Count(x => x.EquivalenceId == idNoApply),
                CountNoSuccess = d.Count(x => x.EquivalenceId == idNoSuccess),
                ValuationsCount = d.Count(),
                d.Key.IdQuestion,
                d.Key.Text,
            }).ToList();

            var AdherenceQuestionPercent = AdherencePatients.Select(x => new
           ReportQuestionAdherenceResponse(
                x.CountNoApply,
                x.CountSuccess,
                x.CountNoSuccess,
                x.ValuationsCount,
                x.IdQuestion,
                x.Text,
                 x.CountNoSuccess + x.CountSuccess == 0 ? 0 : x.CountSuccess * 100 / (x.CountNoSuccess + x.CountSuccess)
            )).ToList();

            return Results.Ok(Result<List<ReportQuestionAdherenceResponse>>.Success(AdherenceQuestionPercent, "Se obtuvo la evaluación correctamente"));
        }
    }
    public class ReportQuestionAdherenceValidator : AbstractValidator<ReportQuestionAdherenceQuery>
    {
        public ReportQuestionAdherenceValidator()
        {
            RuleFor(x => x.IdDataCut).NotEmpty().WithMessage("El id del corte es requerido");
            RuleFor(x => x.IdInstitution).NotEmpty().WithMessage("El id de la institución es requerido");
            RuleFor(x => x.IdGuide).NotEmpty().WithMessage("El id de la guia es requerido");
        }
    }
}
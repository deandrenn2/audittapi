using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Auditt.Reports.Infrastructure.Report;
using Carter;
using Carter.ModelBinding;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class GenerateReportGerencial : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/reports/{idDataCut:int}", async (int idDataCut, IMediator mediator, int idInstitution, int idGuide, string templateName = "") =>
        {
            return await mediator.Send(new GenerateReportCommand(templateName, idDataCut, idInstitution, idGuide));
        })
        .WithName(nameof(GenerateReportGerencial))
        .WithTags("Reports")
        .Produces(StatusCodes.Status200OK)
        .Produces<GenerateReportResponse>(StatusCodes.Status200OK);
    }

    public record GenerateReportCommand(string TemplateName, int IdDataCut, int IdInstitution, int IdGuide) : IRequest<IResult>;

    public record QuestionAdherenceModel(string Text, string PercentSuccess);

    public record GenerateReportResponse(byte[] Report);

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

    public record ReportFunctionaryQuestion(int? IdQuestion, string Text, int CountSuccess, int CountNoApply, int CountNoSuccess, int ValorationsCount, string PercentSuccess);
    public record ReportFunctionaryGrouped(int FunctionaryId, string FunctionaryName, List<ReportFunctionaryQuestion> Questions);

    public record DataQuery
    {
        public int IdDataCut { get; set; }
        public string DateRange { get; set; } = string.Empty;
        public int IdInstitution { get; set; }
        public int IdGuide { get; set; }
        public string GuideName { get; set; } = string.Empty;
        public string GuideDescription { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public int CountHistories { get; set; }
        public string GlobalAdherence { get; set; } = string.Empty;
        public string StrictAdherence { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string AssistantManagerName { get; set; } = string.Empty;
        public List<QuestionAdherenceModel> QuestionAdherence { get; set; } = new List<QuestionAdherenceModel>();
        public List<ReportFunctionaryGrouped> Functionaries { get; set; } = new List<ReportFunctionaryGrouped>();

    }

    public class GenerateReportHandler(AppDbContext context, IValidator<GenerateReportCommand> validator, IClosedXmlReportManager reportManager) : IRequestHandler<GenerateReportCommand, IResult>
    {
        public async Task<IResult> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Assessment.ErrorValidation", "Se presentaron errores de validación")));
            }

            /* Reporte general */

            var assessments = await context.Assessments
                .Include(x => x.Valuations)
                .Include(x => x.Guide)
                .Include(x => x.Institution)
                .Include(x => x.DataCut)
                .Include(x => x.Functionary)
                .Where(x =>
             x.DataCutId == request.IdDataCut
            && x.InstitutionId == request.IdInstitution
            && x.GuideId == request.IdGuide
            ).ToListAsync(cancellationToken);

            var idScale = assessments.First().Guide.ScaleId;

            var scale = await context.Scales.Include(x => x.Equivalences).FirstOrDefaultAsync(x => x.Id == idScale, cancellationToken);
            var dataCut = assessments.First().DataCut;
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
                CountHistories = AdherencePatientsPercent.Count(),
                CountHistoriesStrictAdherence = AdherencePatientsPercent.Count(x => x.percentSuccess == 100),
                GlobalAdherence = AdherencePatientsPercent.Sum(x => x.percentSuccess) / AdherencePatientsPercent.Count(),
            };

            var AdherenceQuestions = assessments.SelectMany(z => z.Valuations).GroupBy(x => new
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

            var AdherenceQuestionPercent = AdherenceQuestions.Select(x => new
           QuestionAdherenceModel(
                x.Text,
                 (x.CountNoSuccess + x.CountSuccess == 0 ? 0 : x.CountSuccess * 100 / (x.CountNoSuccess + x.CountSuccess)).ToString() + '%'
            )).ToList();

            /* Reporte por Funcionarios */
            var AdherenceFunctionaries = assessments.SelectMany(z => z.Valuations).GroupBy(x => new
            {
                x.Assessment.FunctionaryId,
                x.Assessment.Functionary.LastName,
                x.Assessment.Functionary.FirstName,
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
                d.Key.FunctionaryId,
                d.Key.LastName,
                d.Key.FirstName
            }).ToList();

            var groupedFunctionaries = AdherenceFunctionaries
                .GroupBy(x => new { x.FunctionaryId, FunctionaryName = x.LastName + " " + x.FirstName })
                .Select(g => new ReportFunctionaryGrouped(
                    g.Key.FunctionaryId,
                    g.Key.FunctionaryName,
                    g.Select(q => new ReportFunctionaryQuestion(
                        q.IdQuestion,
                        q.Text,
                        q.CountSuccess,
                        q.CountNoApply,
                        q.CountNoSuccess,
                        q.ValuationsCount,
                        (q.CountNoSuccess + q.CountSuccess == 0 ? 0 : q.CountSuccess * 100 / (q.CountNoSuccess + q.CountSuccess)).ToString() + '%'
                    )).ToList()
                )).ToList();

            var res = new DataQuery
            {
                QuestionAdherence = AdherenceQuestionPercent,
                GlobalAdherence = AdherenceGlobal.GlobalAdherence.ToString() + '%',
                StrictAdherence = AdherenceGlobal.StrictAdherence.ToString() + '%',
                CountHistories = AdherenceGlobal.CountHistories,
                DateRange = dataCut.InitialDate.ToString("dd-MM-yyyy") + " - " + dataCut.FinalDate.ToString("dd-MM-yyyy"),
                IdDataCut = request.IdDataCut,
                IdInstitution = request.IdInstitution,
                IdGuide = request.IdGuide,
                GuideName = assessments.First()?.Guide?.Name ?? "",
                InstitutionName = assessments.First()?.Institution?.Name ?? "",
                GuideDescription = assessments.First()?.Guide?.Description ?? "",
                ManagerName = assessments.First()?.Institution?.Manager ?? "",
                AssistantManagerName = assessments.First()?.Institution?.AssistantManager ?? "",
                Functionaries = groupedFunctionaries
            };

            var resArray = reportManager.GenerateReportAsync("ReportGerencial", res);
            var name = string.IsNullOrEmpty(request.TemplateName) ? $"ReportGerencial_{res.IdGuide}_{res.IdDataCut}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}.xlsx" : request.TemplateName;

            return Results.File(resArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name);
        }
    }
    public class GenerateReportValidator : AbstractValidator<GenerateReportCommand>
    {
        public GenerateReportValidator()
        {
            RuleFor(x => x.IdDataCut).NotEmpty().WithMessage("El id del corte es requerido");
            RuleFor(x => x.IdInstitution).NotEmpty().WithMessage("El id de la institución es requerido");
            RuleFor(x => x.IdGuide).NotEmpty().WithMessage("El id de la guia es requerido");
        }
    }

}
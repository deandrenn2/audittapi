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

namespace Auditt.Application.Features.Assessments;

public class GetAssessmentByPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/assessments/patients/{identity}", async (HttpRequest req, IMediator mediator, string identity, int idDataCut, int idFunctionary, int idPatient, int idInstitution, int idGuide) =>
        {
            return await mediator.Send(new GetAssessmentByPatientQuery(identity, idDataCut, idFunctionary, idPatient, idInstitution, idGuide));
        })
        .WithName(nameof(GetAssessmentByPatient))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<GetAssessmentByPatientResponse>(StatusCodes.Status200OK);
    }
    public record GetAssessmentByPatientQuery(string Identification, int IdDataCut, int IdFunctionary, int IdPatient, int IdInstitution, int IdGuide) : IRequest<IResult>;
    public record GetAssessmentByPatientResponse(int Id, int IdDataCut, int IdFunctionary, int IdPatient, string YearOld, DateTime Date, string Eps, int IdUserCreated, int IdUserUpdate, DateTime UpdateDate, DateTime CreateDate, List<ValuationModel> Valuations, int IdScale);
    public record ValuationModel(int Id, int Order, string Text, int IdAssessment, int IdEquivalence, int? IdQuestion);
    public class GetAssessmentByPatientHandler(AppDbContext context, IValidator<GetAssessmentByPatientQuery> validator) : IRequestHandler<GetAssessmentByPatientQuery, IResult>
    {
        public async Task<IResult> Handle(GetAssessmentByPatientQuery request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Assessment.ErrorValidation", "Se presentaron errores de validación")));
            }

            var assessment = await context.Assessments.Include(x => x.Valuations).Include(x => x.Guide).Include(x => x.Patient)
            .Where(x =>
            x.Patient.Identification == request.Identification.Trim()
            && x.DataCutId == request.IdDataCut
            && x.FunctionaryId == request.IdFunctionary
            && x.InstitutionId == request.IdInstitution
            && x.GuideId == request.IdGuide
            ).FirstOrDefaultAsync(cancellationToken);

            if (assessment == null)
            {
                return Results.Ok(Result<IResult>.Failure(Results.NotFound(), new Error("Assessment.NotFound", "No se encontró la evaluación")));
            }

            var valuations = assessment.Valuations.Select(x => new ValuationModel(x.Id, x.Order, x.Text, x.AssessmentId, x.EquivalenceId, x.IdQuestion)).ToList();


            var response = new GetAssessmentByPatientResponse(assessment.Id, assessment.DataCutId, assessment.FunctionaryId, assessment.PatientId, assessment.YearOld, assessment.Date, assessment.Eps, assessment.IdUserCreated, assessment.IdUserUpdate, assessment.UpdateDate, assessment.CreateDate, valuations, assessment.Guide.ScaleId);
            return Results.Ok(Result<GetAssessmentByPatientResponse>.Success(response, "Se obtuvo la evaluación correctamente"));
        }
    }
    public class GetAssessmentByPatientValidator : AbstractValidator<GetAssessmentByPatientQuery>
    {
        public GetAssessmentByPatientValidator()
        {
            RuleFor(x => x.Identification).NotEmpty().WithMessage("El id del paciente es requerido");
            RuleFor(x => x.IdDataCut).NotEmpty().WithMessage("El id del corte es requerido");
            RuleFor(x => x.IdFunctionary).NotEmpty().WithMessage("El id del funcionario es requerido");
            RuleFor(x => x.IdPatient).NotEmpty().WithMessage("El id del paciente es requerido");
            RuleFor(x => x.IdInstitution).NotEmpty().WithMessage("El id de la institución es requerido");
            RuleFor(x => x.IdGuide).NotEmpty().WithMessage("El id de la guia es requerido");
        }
    }
}
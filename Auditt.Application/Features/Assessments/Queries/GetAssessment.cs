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

public class GetAssessment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/assessments/{id}", async (HttpRequest req, IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetAssessmentQuery(id));
        })
        .WithName(nameof(GetAssessment))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<GetAssessmentResponse>(StatusCodes.Status200OK);
    }
    public record GetAssessmentQuery(int Id) : IRequest<IResult>;
    public record GetAssessmentResponse(
        int Id,
        int IdDataCut,
        int IdFunctionary,
        string FunctionaryName,
        int IdPatient,
        string Identification,
        string YearOld,
        DateTime Date,
        string Eps,
        int IdUserCreated,
        int IdUserUpdate,
        DateTime UpdateDate,
        DateTime CreateDate,
        int IdGuide,
        string GuideName,
        List<ValuationModel> Valuations);
    public record ValuationModel(int Id, int Order, string Text, int IdAssessment, int IdEquivalence, int? IdQuestion);
    public class GetAssessmentHandler(AppDbContext context, IValidator<GetAssessmentQuery> validator) : IRequestHandler<GetAssessmentQuery, IResult>
    {
        public async Task<IResult> Handle(GetAssessmentQuery request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var assessment = await context.Assessments
                        .Include(x => x.Valuations)
                        .Include(x => x.Functionary)
                        .Include(x => x.Patient)
                        .Include(x => x.Guide)
                        .Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);



            if (assessment == null)
            {
                return Results.NotFound(Result<IResult>.Failure(Results.NotFound(), new Error("Assessment.NotFound", "No se encontró la evaluación")));
            }

            var valuations = assessment.Valuations.Select(x => new ValuationModel(x.Id, x.Order, x.Text, x.AssessmentId, x.EquivalenceId, x.IdQuestion)).ToList();


            var response = new GetAssessmentResponse(
                assessment.Id,
                assessment.DataCutId,
                assessment.FunctionaryId,
                assessment.Functionary?.FirstName + " " + (assessment.Functionary?.LastName ?? string.Empty),
                assessment.PatientId,
                assessment.Patient?.Identification ?? string.Empty,
                assessment.YearOld,
                assessment.Date,
                assessment.Eps,
                assessment.IdUserCreated,
                assessment.IdUserUpdate,
                assessment.UpdateDate,
                assessment.CreateDate,
                assessment.GuideId,
                assessment.Guide?.Name ?? string.Empty,
                valuations
                );
            return Results.Ok(Result<GetAssessmentResponse>.Success(response, "Se obtuvo la evaluación correctamente"));
        }
    }
    public class GetAssessmentValidator : AbstractValidator<GetAssessmentQuery>
    {
        public GetAssessmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id no puede estar vacío");
        }
    }
}
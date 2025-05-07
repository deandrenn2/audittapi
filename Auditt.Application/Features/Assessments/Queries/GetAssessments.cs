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

public class GetAssessments : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/assessments/{idInstitution:int}", async (HttpRequest req, IMediator mediator, int idInstitution) =>
        {
            return await mediator.Send(new GetAssessmentsQuery(idInstitution));
        })
        .WithName(nameof(GetAssessments))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<List<GetAssessmentsResponse>>(StatusCodes.Status200OK);
    }
    public record GetAssessmentsQuery(int InstitutionId) : IRequest<IResult>;
    public record GetAssessmentsResponse(int Id, string IdentificationPatient, string FunctionaryName, DateTime Date);
    public class GetAssessmentsHandler(AppDbContext context, IValidator<GetAssessmentsQuery> validator) : IRequestHandler<GetAssessmentsQuery, IResult>
    {
        public async Task<IResult> Handle(GetAssessmentsQuery request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var assessments = await context.Assessments.Where(x => x.InstitutionId == request.InstitutionId).Include(x => x.Patient).Include(x => x.Functionary).Include(x => x.Valuations).ToListAsync(cancellationToken);
            var response = assessments.Select(x => new GetAssessmentsResponse(x.Id, x.Patient.Identification, $"{x.Functionary.LastName} {x.Functionary.FirstName}", x.Date)).ToList();
            return Results.Ok(Result<List<GetAssessmentsResponse>>.Success(response, "Se obtuvo la evaluación correctamente"));
        }
    }
    public class GetAssessmentsValidator : AbstractValidator<GetAssessmentsQuery>
    {
        public GetAssessmentsValidator()
        {
            // No validation rules needed for this query
            RuleFor(x => x.InstitutionId).NotEmpty().WithMessage("El id de la institución es requerido");
        }
    }
}
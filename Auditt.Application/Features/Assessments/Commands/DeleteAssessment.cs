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

namespace Auditt.Application.Features.Assessments.Commands;

public class DeleteAssessment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/assessments/{id:int}", async (HttpRequest req, IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteAssessmentCommand { Id = id });
        })
        .WithName(nameof(DeleteAssessment))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<DeleteAssessmentResponse>(StatusCodes.Status200OK);
    }
    public record DeleteAssessmentCommand : IRequest<IResult>
    {
        public int Id { get; set; }
    }
    public record DeleteAssessmentResponse(int Id);
    public class DeleteAssessmentHandler(AppDbContext context, IValidator<DeleteAssessmentCommand> validator) : IRequestHandler<DeleteAssessmentCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteAssessmentCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var assessment = await context.Assessments.FindAsync(request.Id);
            if (assessment == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDelete", "No se encontró el registro")));
            }
            context.Assessments.Remove(assessment);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeleteAssessmentResponse(assessment.Id);
                return Results.Ok(Result<DeleteAssessmentResponse>.Success(resModel, "Evaluación eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDelete", "Error al eliminar la evaluación")));
            }
        }
    }
    public class DeleteAssessmentValidator : AbstractValidator<DeleteAssessmentCommand>
    {
        public DeleteAssessmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id es requerido");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El id es requerido");
        }
    }
}
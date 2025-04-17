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

namespace Auditt.Application.Features.Questions;

public class DeleteQuestion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/questions/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteQuestionCommand(id));
        })
        .WithName(nameof(DeleteQuestion))
        .WithTags(nameof(Question))
        .ProducesValidationProblem()
        .Produces<DeleteQuestionResponse>(StatusCodes.Status200OK);
    }
    public record DeleteQuestionCommand(int Id) : IRequest<IResult>;
    public record DeleteQuestionResponse(int Id);
    public class DeleteQuestionHandler(AppDbContext context, IValidator<DeleteQuestionCommand> validator) : IRequestHandler<DeleteQuestionCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var question = await context.Questions.FindAsync(request.Id);
            if (question == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Pregunta no encontrada")));
            }
            context.Questions.Remove(question);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeleteQuestionResponse(question.Id);
                return Results.Ok(Result<DeleteQuestionResponse>.Success(resModel, "Pregunta eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDelete", "Error al eliminar la pregunta")));
            }
        }
    }
    public class DeleteQuestionValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la pregunta debe ser mayor que 0");
        }
    }
}
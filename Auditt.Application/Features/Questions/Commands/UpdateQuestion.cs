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

public class UpdateQuestion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/questions", async (IMediator mediator, UpdateQuestionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateQuestion))
        .WithTags(nameof(Question))
        .ProducesValidationProblem()
        .Produces<UpdateQuestionResponse>(StatusCodes.Status200OK);
    }
    public record UpdateQuestionCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public string Text { get; init; } = string.Empty;
        public int IdUser { get; init; } 
    }
    public record UpdateQuestionResponse(int Id, string Text, int IdGuide);
    public class UpdateQuestionHandler(AppDbContext context, IValidator<UpdateQuestionCommand> validator) : IRequestHandler<UpdateQuestionCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
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
            question.Update(request.Text, request.IdUser);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdateQuestionResponse(question.Id, question.Text, question.IdGuide);
                return Results.Ok(Result<UpdateQuestionResponse>.Success(resModel, "Pregunta actualizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdate", "Error al actualizar la pregunta")));
            }
        }
    }
    public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la pregunta debe ser mayor que 0");
            RuleFor(x => x.Text).NotEmpty().WithMessage("El texto de la pregunta es requerido");
        }
    }
}
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

public class CreateQuestion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions", async (IMediator mediator, CreateQuestionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateQuestion))
        .WithTags(nameof(Question))
        .ProducesValidationProblem()
        .Produces<CreateQuestionResponse>(StatusCodes.Status200OK);
    }
    public record CreateQuestionCommand(string Text, int Order, int IdGuide, int IdUser) : IRequest<IResult>;
    public record CreateQuestionResponse(int Id, string Text, int Order, int IdGuide);
    public class CreateQuestionHandler(AppDbContext context, IValidator<CreateQuestionCommand> validator) : IRequestHandler<CreateQuestionCommand, IResult>
    {
        public async Task<IResult> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var question = new Question(0, request.Text, request.Order, request.IdGuide, request.IdUser);
            await context.Questions.AddAsync(question);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateQuestionResponse(question.Id, question.Text,question.Order, question.IdGuide);
                return Results.Ok(Result<CreateQuestionResponse>.Success(resModel, "Pregunta creada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreate", "Error al crear la pregunta")));
            }
        }
    }
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("El nombre de la pregunta es requerido");
            RuleFor(x => x.Order).GreaterThan(0).WithMessage("El orden de la pregunta debe ser mayor que 0");
            RuleFor(x => x.IdGuide).GreaterThan(0).WithMessage("El Id de la guía debe ser mayor que 0");
        }
    }
}
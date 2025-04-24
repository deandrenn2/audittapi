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

public class GetQuestion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/questions/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetQuestionCommand(id));
        })
        .WithName(nameof(GetQuestion))
        .WithTags(nameof(Question))
        .ProducesValidationProblem()
        .Produces<GetQuestionResponse>(StatusCodes.Status200OK);
    }
    public record GetQuestionCommand(int Id) : IRequest<IResult>;
    public record GetQuestionResponse(int Id, string Text, int Order, int IdGuide);
    public class GetQuestionHandler(AppDbContext context, IValidator<GetQuestionCommand> validator) : IRequestHandler<GetQuestionCommand, IResult>
    {
        public async Task<IResult> Handle(GetQuestionCommand request, CancellationToken cancellationToken)
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
            var resModel = new GetQuestionResponse(question.Id, question.Text, question.Order, question.GuideId);
            return Results.Ok(Result<GetQuestionResponse>.Success(resModel, "Pregunta obtenida correctamente"));
        }
    }
    public class GetQuestionValidator : AbstractValidator<GetQuestionCommand>
    {
        public GetQuestionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la pregunta debe ser mayor que 0");
        }
    }
}
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

namespace Auditt.Application.Features.Questions;

public class GetQuestions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/questions/{idGuide:int}", async (IMediator mediator, int idGuide) =>
        {
            return await mediator.Send(new GetQuestionsCommand(idGuide));
        })
        .WithName(nameof(GetQuestions))
        .WithTags(nameof(Question))
        .ProducesValidationProblem()
        .Produces<GetQuestionsResponse>(StatusCodes.Status200OK);
    }
    public record GetQuestionsCommand(int IdGuide) : IRequest<IResult>;
    public record GetQuestionsResponse(int Id, string Text, int Order, int IdGuide);
    public class GetQuestionsHandler(AppDbContext context, IValidator<GetQuestionsCommand> validator) : IRequestHandler<GetQuestionsCommand, IResult>
    {
        public async Task<IResult> Handle(GetQuestionsCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var questions = await context.Questions.Where(x => x.IdGuide == request.IdGuide).ToListAsync();
            var resModel = questions.Select(q => new GetQuestionsResponse(q.Id, q.Text, q.Order, q.IdGuide)).ToList();
            return Results.Ok(Result<List<GetQuestionsResponse>>.Success(resModel, "Preguntas obtenidas correctamente"));
        }
    }
}
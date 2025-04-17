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
public class GetGuides : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/guides", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetGuidesCommand());
        })
        .WithName(nameof(GetGuides))
        .WithTags(nameof(Guide))
        .ProducesValidationProblem()
        .Produces<GetGuidesResponse>(StatusCodes.Status200OK);
    }
    public record GetGuidesCommand() : IRequest<IResult>;
    public record GetGuidesResponse(int Id, string Name, string Description, int IdScale);
    public class GetGuidesHandler(AppDbContext context, IValidator<GetGuidesCommand> validator) : IRequestHandler<GetGuidesCommand, IResult>
    {
        public async Task<IResult> Handle(GetGuidesCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var guides = await context.Guides.ToListAsync();
            var resModel = guides.Select(g => new GetGuidesResponse(g.Id, g.Name, g.Description, g.IdScale)).ToList();
            return Results.Ok(Result<List<GetGuidesResponse>>.Success(resModel, "Guías obtenidas correctamente"));
        }
    }

}
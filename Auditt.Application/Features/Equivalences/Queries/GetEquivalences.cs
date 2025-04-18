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

namespace Auditt.Application.Features.Equivalences;

public class GetEquivalences : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/equivalents", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetEquivalentsQuery());
        })
        .WithName(nameof(GetEquivalences))
        .WithTags(nameof(Equivalence))
        .ProducesValidationProblem()
        .Produces<GetEquivalentsResponse>(StatusCodes.Status200OK);
    }
    public record GetEquivalentsQuery() : IRequest<IResult>;
    public record GetEquivalentsResponse(List<Equivalence> Equivalents);
    public class GetEquivalentsHandler(AppDbContext context) : IRequestHandler<GetEquivalentsQuery, IResult>
    {
        public async Task<IResult> Handle(GetEquivalentsQuery request, CancellationToken cancellationToken)
        {
            var equivalents = await context.Equivalences.ToListAsync(cancellationToken);
            var resModel = new GetEquivalentsResponse(equivalents);
            return Results.Ok(Result<List<Equivalence>>.Success(equivalents, "Escalas obtenidas correctamente"));
        }
    }
}
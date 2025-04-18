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

namespace Auditt.Application.Features.Equivalences;

public class GetEquivalence : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/equivalents/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetEquivalentQuery(id));
        })
        .WithName(nameof(GetEquivalence))
        .WithTags(nameof(Equivalence))
        .ProducesValidationProblem()
        .Produces<GetEquivalenceResponse>(StatusCodes.Status200OK);
    }
    public record GetEquivalentQuery(int Id) : IRequest<IResult>;
    public record GetEquivalenceResponse(Equivalence Equivalence);
    public class GetEquivalentHandler(AppDbContext context) : IRequestHandler<GetEquivalentQuery, IResult>
    {
        public async Task<IResult> Handle(GetEquivalentQuery request, CancellationToken cancellationToken)
        {
            var equivalent = await context.Equivalences.FindAsync(request.Id);
            if (equivalent == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Escala no encontrada")));
            }
            var resModel = new GetEquivalenceResponse(equivalent);
            return Results.Ok(Result<Equivalence>.Success(equivalent, "Escala obtenida correctamente"));
        }
    }
}
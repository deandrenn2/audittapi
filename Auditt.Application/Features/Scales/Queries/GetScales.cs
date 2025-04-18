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

namespace Auditt.Application.Features.Scales;

public class GetScales : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/scales", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetScalesQuery());
        })
        .WithName(nameof(GetScales))
        .WithTags(nameof(Scale))
        .ProducesValidationProblem()
        .Produces<GetScalesResponse>(StatusCodes.Status200OK);
    }
    public record GetScalesQuery() : IRequest<IResult>;
    public record GetScalesResponse(List<Scale> Scales);
    public class GetScalesHandler(AppDbContext context) : IRequestHandler<GetScalesQuery, IResult>
    {
        public async Task<IResult> Handle(GetScalesQuery request, CancellationToken cancellationToken)
        {
            var scales = await context.Scales.ToListAsync();
            var resModel = new GetScalesResponse(scales);
            return Results.Ok(Result<List<Scale>>.Success(scales, "Escalas obtenidas correctamente"));
        }
    }
}
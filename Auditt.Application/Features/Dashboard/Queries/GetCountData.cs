using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;

public class GetCountData : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/dashboard/count", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetCountDataRequest());
        })
         .WithName(nameof(GetCountData))
        .WithTags("Dashboard")
        .ProducesValidationProblem()
        .Produces<GetCountDataResponse>(StatusCodes.Status200OK);
    }


    public record GetCountDataRequest() : IRequest<IResult>;

    public record GetCountDataResponse(Int32 ValuationsCount, Int32 PatientsCount, Int32 FunctionariesCount);

    public class GetCountDataHandler(AppDbContext context) : IRequestHandler<GetCountDataRequest, IResult>
    {
        public async Task<IResult> Handle(GetCountDataRequest request, CancellationToken cancellationToken)
        {
            var patientsCount = await context.Patients.CountAsync(cancellationToken);
            var valuationsCount = await context.Valuations.CountAsync(cancellationToken);
            var functionariesCount = await context.Functionaries.CountAsync(cancellationToken);

            return Results.Ok(Result<GetCountDataResponse>.Success(new GetCountDataResponse(valuationsCount, patientsCount, functionariesCount), "OK"));
        }
    }
}
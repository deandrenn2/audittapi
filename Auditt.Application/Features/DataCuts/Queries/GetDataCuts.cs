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

namespace Auditt.Application.Features.DataCuts;

public class GetDataCuts : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/datacuts", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetDataCutsQuery());
        })
        .WithName(nameof(GetDataCuts))
        .WithTags(nameof(DataCut))
        .ProducesValidationProblem()
        .Produces<GetDataCutsResponse>(StatusCodes.Status200OK);
    }
    public record GetDataCutsQuery() : IRequest<IResult>;
    public record GetDataCutsResponse(int Id, string Name, string Cycle, DateTime InitialDate, DateTime FinalDate, int MaxHistory, int InstitutionId);
    public class GetDataCutsHandler(AppDbContext context) : IRequestHandler<GetDataCutsQuery, IResult>
    {
        public async Task<IResult> Handle(GetDataCutsQuery request, CancellationToken cancellationToken)
        {
            var dataCuts = await context.DataCuts.ToListAsync(cancellationToken);
            var response = dataCuts.Select(dataCut => new GetDataCutsResponse(dataCut.Id, dataCut.Name, dataCut.Cycle, dataCut.InitialDate, dataCut.FinalDate, dataCut.MaxHistory, dataCut.InstitutionId)).ToList();
            return Results.Ok(Result<List<GetDataCutsResponse>>.Success(response, "Lista de cortes de datos"));
        }
    }
}

using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Features.Functionaries;

public class GetFunctionaries : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/functionaries", async (HttpRequest req, IMediator mediator) =>
        {
            return await mediator.Send(new GetFunctionariesQuery());
        })
        .WithName(nameof(GetFunctionaries))
        .WithTags(nameof(Functionary))
        .ProducesValidationProblem()
        .Produces<GetFunctionariesResponse>(StatusCodes.Status200OK);
    }
    public record GetFunctionariesQuery : IRequest<IResult>;
    public record GetFunctionariesResponse(int Id, string FirstName, string LastName, string Identification);
    public class GetFunctionariesHandler(AppDbContext context) : IRequestHandler<GetFunctionariesQuery, IResult>
    {
        public async Task<IResult> Handle(GetFunctionariesQuery request, CancellationToken cancellationToken)
        {
            var functionaries = await context.Functionaries.ToListAsync();
            var resModel = functionaries.Select(x => new GetFunctionariesResponse(x.Id,x.FirstName, x.LastName, x.Identification)).ToList();
            return Results.Ok(Result<List<GetFunctionariesResponse>>.Success(resModel, "Lista de funcionarios obtenida correctamente"));
        }
    }
}
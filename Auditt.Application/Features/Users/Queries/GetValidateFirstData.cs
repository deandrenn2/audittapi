using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;

namespace Auditt.Application.Features.Users;
public class GetValidateFirstData : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/first/data", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetValidateFirstDataQuery());
        })
             .WithName(nameof(GetValidateFirstData))
             .WithTags(nameof(User))
             .ProducesValidationProblem()
             .Produces<bool>(StatusCodes.Status200OK);
    }

    public record GetValidateFirstDataQuery() : IRequest<IResult>;

    public class GetValidateFirstDataHandler(AppDbContext contex) : IRequestHandler<GetValidateFirstDataQuery, IResult>
    {
        public async Task<IResult> Handle(GetValidateFirstDataQuery request, CancellationToken cancellationToken)
        {
            var hasUsers = await contex.Users.FirstOrDefaultAsync();

            if (hasUsers == null)
            {
                return Results.Ok(Result<bool>.Success(false, "No existe usuarios en la base de datos"));
            }

            return Results.Ok(Result<bool>.Success(true, "Datos de usuarios y empresas cargados"));
        }
    }
}
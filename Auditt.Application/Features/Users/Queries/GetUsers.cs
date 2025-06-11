using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;

namespace Auditt.Application.Features.Users.Queries;

public class GetUsers : ICarterModule
{
    public record GetUsersResponse(int Id, string? FirstName, string? LastName, int IdEstado, string Email, int IdRol, string? UrlProfile, string? RoleName);

    public record GetUsersQuery() : IRequest<Result>;

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetUsersQuery());
        })
        .WithName(nameof(GetUsers))
        .WithTags(nameof(User))
        .Produces(StatusCodes.Status200OK);
    }

    public class GetUserHandler(AppDbContext context) : IRequestHandler<GetUsersQuery, Result>
    {
        public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await context.Users.Include(x => x.Role).ToListAsync(cancellationToken);

            var userList = users.Select(x => new GetUsersResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                x.StatusId,
                x.Email,
                x.RoleId,
                x.UrlProfile,
                x.Role.Name
                )).ToList();

            return Result<List<GetUsersResponse>>.Success(userList, "Listado de usuarios");
        }
    }
}


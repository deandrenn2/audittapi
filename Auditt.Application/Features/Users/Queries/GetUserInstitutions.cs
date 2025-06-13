using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;

namespace Auditt.Application.Features.Users.Queries;

public class GetUserInstitutions : ICarterModule
{
    public record UserInstitutionsResponse(int Id, string Name, string Nit, int IdState);

    public record GetUserInstitutionsQuery(int Id) : IRequest<Result>;

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/{id}/institutions", async (int id, IMediator mediator) =>
        {
            return await mediator.Send(new GetUserInstitutionsQuery(id));
        })
        .WithName(nameof(GetUserInstitutions))
        .WithTags(nameof(User))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
    public class GetUserInstitutionsHandler(AppDbContext context) : IRequestHandler<GetUserInstitutionsQuery, Result>
    {
        public async Task<Result> Handle(GetUserInstitutionsQuery request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Institutions)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (user == null)
            {
                return Result.Failure(new Error("User.ErrorData", "El id de usuario no existe"));
            }

            var institutions = user.Institutions.Select(i => new UserInstitutionsResponse(i.Id, i.Name, i.Nit, i.StatusId)).ToList();

            return Result<List<UserInstitutionsResponse>>.Success(institutions, "Datos de las instituciones del usuario");
        }
    }


}
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


namespace Auditt.Application.Features.Institutions;

public class GetInstitutions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/institutions", async (IMediator mediator, int? idUser) =>
        {
            return await mediator.Send(new GetInstitutionsQuery(idUser));
        })
        .WithName(nameof(GetInstitutions))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<List<GetInstitutionsResponse>>(StatusCodes.Status200OK);
    }
    public record GetInstitutionsQuery(int? IdUser) : IRequest<IResult>;
    public record GetInstitutionsResponse(int Id, string Name, string Abbreviation, string Nit, string City, int IdState);
    public class GetInstitutionsHandler(AppDbContext context) : IRequestHandler<GetInstitutionsQuery, IResult>
    {
        public async Task<IResult> Handle(GetInstitutionsQuery request, CancellationToken cancellationToken)
        {
            var institutions = new List<Institution>();
            if (request.IdUser.HasValue)
            {
                var user = await context.Users
                    .Include(u => u.Institutions)
                    .FirstOrDefaultAsync(x => x.Id == request.IdUser.Value);

                if (user == null)
                {
                    return Results.NotFound(Result.Failure(new Error("User.ErrorData", "El id de usuario no existe")));
                }

                institutions = user.Institutions.Where(x => x.StatusId == 1).ToList();
            }
            else
            {
                institutions = await context.Institutions.ToListAsync();
            }
            if (institutions == null || !institutions.Any())
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorGetInstitucion", "Error al obtener la institución")));
            }
            var resModel = institutions.Select(i => new GetInstitutionsResponse(i.Id, i.Name, i.Abbreviation, i.Nit, i.City, i.StatusId)).ToList();
            return Results.Ok(Result<List<GetInstitutionsResponse>>.Success(resModel, "Institución obtenida correctamente"));
        }
    }
}
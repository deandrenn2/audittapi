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
        app.MapGet("api/institutions", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetInstitutionsQuery());
        })
        .WithName(nameof(GetInstitutions))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<GetInstitutionsResponse>(StatusCodes.Status200OK);
    }
    public record GetInstitutionsQuery() : IRequest<IResult>;
    public record GetInstitutionsResponse(string Name, string Abbreviation, string Nit, string City);
    public class GetInstitutionsHandler(AppDbContext context) : IRequestHandler<GetInstitutionsQuery, IResult>
    {
        public async Task<IResult> Handle(GetInstitutionsQuery request, CancellationToken cancellationToken)
        {
            var institutions = await context.Institutions.ToListAsync();
            if (institutions == null || !institutions.Any())
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorGetInstitucion", "Error al obtener la institución")));
            }
            var resModel = institutions.Select(i => new GetInstitutionsResponse(i.Name, i.Abbreviation, i.Nit, i.City)).ToList();
            return Results.Ok(Result<List<Institution>>.Success(institutions, "Institución obtenida correctamente"));
        }
    }
}   
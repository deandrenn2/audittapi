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
public class GetInstitution : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/institutions/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetInstitutionQuery(id));
        })
        .WithName(nameof(GetInstitution))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<GetInstitutionResponse>(StatusCodes.Status200OK);
    }
    public record GetInstitutionQuery(int Id) : IRequest<IResult>;
    public record GetInstitutionResponse(string Name, string Abbreviation, string Nit, string City);
    public class GetInstitutionHandler(AppDbContext context) : IRequestHandler<GetInstitutionQuery, IResult>
    {
        public async Task<IResult> Handle(GetInstitutionQuery request, CancellationToken cancellationToken)
        {
            var institution = await context.Institutions.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            if (institution == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorGetInstitucion", "Error al obtener la institución")));
            }
            var resModel = new GetInstitutionResponse(institution.Name, institution.Abbreviation, institution.Nit, institution.City);
            return Results.Ok(Result<Institution>.Success(institution, "Institución obtenida correctamente"));
        }
    }

    public class GetInstitutionValidator : AbstractValidator<GetInstitutionQuery>
    {
        public GetInstitutionValidator()
        {
           RuleFor(x => x.Id).NotEmpty().WithMessage("El id de la institución es requerido");
        }
    }
}

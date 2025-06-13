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

namespace Auditt.Application.Features.Institutions;

public class AddInstitution : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/institutions", async (HttpRequest req, IMediator mediator, AddInstitutionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(AddInstitution))
        .WithTags(nameof(Users))
        .ProducesValidationProblem()
        .Produces<AddInstitutionResponse>(StatusCodes.Status200OK);
    }

    public record AddInstitutionCommand(int IdUser, int IdInstitution) : IRequest<IResult>;

    public record AddInstitutionResponse(int Id, string Name, string Abbreviation, string Nit, string City);

    public class AddInstitutionHandler(AppDbContext context, IValidator<AddInstitutionCommand> validator) : IRequestHandler<AddInstitutionCommand, IResult>
    {
        public async Task<IResult> Handle(AddInstitutionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<Dictionary<string, string[]>>.Failure(
                result.GetValidationProblems(),
                new Error("Institution.ErrorValidation", "Se presentaron errores de validación")
            ));
            }

            var user = await context.Users.FindAsync(request.IdUser);
            if (user == null)
            {
                return Results.Ok(Result.Failure(new Error("User.ErrorNotFound", "Usuario no encontrado")));
            }
            var institution = await context.Institutions.FindAsync(request.IdInstitution);
            if (institution == null)
            {
                return Results.Ok(Result.Failure(new Error("Institution.ErrorNotFound", "Institución no encontrada")));
            }
            if (user.Institutions.Any(i => i.Id == institution.Id))
            {
                return Results.Ok(Result.Failure(new Error("Institution.ErrorAlreadyExists", "La institución ya está asociada al usuario")));
            }

            user.Institutions.Add(institution);
            var resCount = await context.SaveChangesAsync(cancellationToken);
            if (resCount > 0)
            {
                return Results.Ok(Result<AddInstitutionResponse>.Success(
                    new AddInstitutionResponse(institution.Id, institution.Name, institution.Abbreviation, institution.Nit, institution.City),
                    "Institución creada y asociada al usuario"
                ));
            }
            return Results.Ok(Result.Failure(new Error("Institution.ErrorCreate", "Error al crear la institución")));
        }
    }

    public class AddInstitutionValidator : AbstractValidator<AddInstitutionCommand>
    {
        public AddInstitutionValidator()
        {
            RuleFor(x => x.IdInstitution)
                .GreaterThan(0).WithMessage("El ID de la institución debe ser mayor que 0");

            RuleFor(x => x.IdUser)
                .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que 0");
        }
    }
}

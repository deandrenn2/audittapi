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

public class RemoveInstitution : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/users/{IdUser}/institutions/{IdInstitution}", async (int IdUser, int IdInstitution, IMediator mediator) =>
        {
            return await mediator.Send(new RemoveInstitutionCommand(IdUser, IdInstitution));
        })
        .WithName(nameof(RemoveInstitution))
        .WithTags(nameof(Users))
        .ProducesValidationProblem()
        .Produces<RemoveInstitutionResponse>(StatusCodes.Status200OK);
    }

    public record RemoveInstitutionCommand(int IdUser, int IdInstitution) : IRequest<IResult>;

    public record RemoveInstitutionResponse(int Id);

    public class RemoveInstitutionHandler(AppDbContext context, IValidator<RemoveInstitutionCommand> validator) : IRequestHandler<RemoveInstitutionCommand, IResult>
    {
        public async Task<IResult> Handle(RemoveInstitutionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<Dictionary<string, string[]>>.Failure(
                    result.GetValidationProblems(),
                    new Error("Institution.ErrorValidation", "Se presentaron errores de validación")
                ));
            }

            var user = await context.Users.Include(u => u.Institutions).FirstOrDefaultAsync(u => u.Id == request.IdUser);
            if (user == null)
            {
                return Results.Ok(Result.Failure(new Error("User.ErrorNotFound", "Usuario no encontrado")));
            }
            var institution = await context.Institutions.FindAsync(request.IdInstitution);
            if (institution == null)
            {
                return Results.Ok(Result.Failure(new Error("Institution.ErrorNotFound", "Institución no encontrada")));
            }
            if (!user.Institutions.Any(i => i.Id == institution.Id))
            {
                return Results.Ok(Result.Failure(new Error("Institution.ErrorNotAssociated", "La institución no está asociada al usuario")));
            }
            user.Institutions.Remove(institution);
            var resCount = await context.SaveChangesAsync(cancellationToken);
            if (resCount > 0)
            {
                return Results.Ok(Result.Success("Institución eliminada correctamente"));
            }
            return Results.Ok(Result.Failure(new Error("Institution.ErrorDelete", "Error al eliminar la institución")));
        }
    }

    public class RemoveInstitutionValidator : AbstractValidator<RemoveInstitutionCommand>
    {
        public RemoveInstitutionValidator()
        {
            RuleFor(x => x.IdUser).GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que 0.");
            RuleFor(x => x.IdInstitution).GreaterThan(0).WithMessage("El ID de la institución debe ser mayor que 0.");
        }
    }
}
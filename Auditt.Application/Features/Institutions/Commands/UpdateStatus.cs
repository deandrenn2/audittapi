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

public class UpdateStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/institutions/status", async (HttpRequest req, IMediator mediator, UpdateStatusCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateStatus))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<UpdateStatusResponse>(StatusCodes.Status200OK);
    }

    public record UpdateStatusCommand(int Id, int StatusId) : IRequest<IResult>;

    public record UpdateStatusResponse(int Id, int StatusId);

    public class UpdateStatusHandler(AppDbContext context, IValidator<UpdateStatusCommand> validator) : IRequestHandler<UpdateStatusCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<Dictionary<string, string[]>>.Failure(
                result.GetValidationProblems(),
                new Error("Institution.ErrorValidation", "Se presentaron errores de validación")
            ));
            }

            var institution = await context.Institutions.FindAsync(request.Id);
            if (institution == null)
            {
                return Results.Ok(Result.Failure(new Error("Institution.ErrorNotFound", "Institución no encontrada")));
            }

            institution.ChangeStatus(request.StatusId);
            var resCount = await context.SaveChangesAsync(cancellationToken);
            if (resCount > 0)
            {
                return Results.Ok(new UpdateStatusResponse(institution.Id, institution.StatusId));
            }

            return Results.Ok(Result.Failure(new Error("Institution.ErrorUpdate", "Error al actualizar el estado de la institución")));
        }
    }

    public class UpdateStatusValidator : AbstractValidator<UpdateStatusCommand>
    {
        public UpdateStatusValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la institución debe ser mayor que 0.");
            RuleFor(x => x.StatusId).InclusiveBetween(1, 3).WithMessage("El estado debe estar entre 1 y 3.");
        }
    }
}


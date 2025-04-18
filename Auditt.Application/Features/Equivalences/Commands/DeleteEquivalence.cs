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

namespace Auditt.Application.Features.Equivalences;

public class DeleteEquivalence : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/equivalents/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteEquivalentCommand(id));
        })
        .WithName(nameof(DeleteEquivalence))
        .WithTags(nameof(Equivalence))
        .ProducesValidationProblem()
        .Produces<DeleteEquivalentResponse>(StatusCodes.Status200OK);
    }
    public record DeleteEquivalentCommand(int Id) : IRequest<IResult>;
    public record DeleteEquivalentResponse(int Id);
    public class DeleteEquivalentHandler(AppDbContext context, IValidator<DeleteEquivalentCommand> validator) : IRequestHandler<DeleteEquivalentCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteEquivalentCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var equivalent = await context.Equivalences.FindAsync(request.Id);
            if (equivalent == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Equivalente no encontrado")));
            }
            context.Equivalences.Remove(equivalent);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeleteEquivalentResponse(equivalent.Id);
                return Results.Ok(Result<Equivalence>.Success(equivalent, "Escala eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al eliminar la escala")));
            }
        }
    }
    public class DeleteEquivalentValidator : AbstractValidator<DeleteEquivalentCommand>
    {
        public DeleteEquivalentValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID del equivalente debe ser mayor que cero.");
        }
    }
}
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

public class UpdateEquivalence : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/equivalents", async (IMediator mediator, UpdateEquivalentCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateEquivalence))
        .WithTags(nameof(Equivalence))
        .ProducesValidationProblem()
        .Produces<UpdateEquivalentResponse>(StatusCodes.Status200OK);
    }
    public record UpdateEquivalentCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public int ScaleId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Value { get; init; } = 0;
    }
    public record UpdateEquivalentResponse(int Id, string Name);
    public class UpdateEquivalentHandler(AppDbContext context, IValidator<UpdateEquivalentCommand> validator) : IRequestHandler<UpdateEquivalentCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateEquivalentCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var scale = await context.Scales.FindAsync(request.ScaleId);
            if (scale == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Escala no encontrada")));
            }
            var equivalent = await context.Equivalences.FindAsync(request.Id);
            if (equivalent == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Equivalente no encontrado")));
            }
            equivalent.Update(request.Name, request.Value);
            context.Equivalences.Update(equivalent);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdateEquivalentResponse(equivalent.Id, equivalent.Name);
                return Results.Ok(Result<Equivalence>.Success(equivalent, "Escala actualizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al crear la escala")));
            }
        }
    }
    public class UpdateEquivalentValidator : AbstractValidator<UpdateEquivalentCommand>
    {
        public UpdateEquivalentValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID del equivalente debe ser mayor que cero.");
            RuleFor(x => x.ScaleId).GreaterThan(0).WithMessage("El ID de la escala debe ser mayor que cero.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre del equivalente no puede estar vacío.");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("El valor del equivalente debe ser mayor que cero.");
        }
    }
}
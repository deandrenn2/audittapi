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

namespace Auditt.Application.Features.Equivalents;

public class CreateEquivalence : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/equivalents", async (IMediator mediator, CreateEquivalentCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateEquivalence))
        .WithTags(nameof(Equivalence))
        .ProducesValidationProblem()
        .Produces<CreateEquivalentResponse>(StatusCodes.Status200OK);
    }
    public record CreateEquivalentCommand : IRequest<IResult>
    {
        public int ScaleId { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Value { get; init; } = 0;
    }
    public record CreateEquivalentResponse(int Id, string Name);
    public class CreateEquivalentHandler(AppDbContext context, IValidator<CreateEquivalentCommand> validator) : IRequestHandler<CreateEquivalentCommand, IResult>
    {
        public async Task<IResult> Handle(CreateEquivalentCommand request, CancellationToken cancellationToken)
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
            var equivalent = Equivalence.Create(0, request.ScaleId, request.Name, request.Value);
            await context.Equivalences.AddAsync(equivalent);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateEquivalentResponse(equivalent.Id, equivalent.Name);
                return Results.Ok(Result<Equivalence>.Success(equivalent, "Escala creada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al crear la escala")));
            }
        }
    }
    public class CreateEquivalentValidator : AbstractValidator<CreateEquivalentCommand>
    {
        public CreateEquivalentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre no puede estar vacío.");
            RuleFor(x => x.ScaleId).GreaterThan(0).WithMessage("El ID de la escala debe ser mayor que cero.");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("El valor debe ser mayor que cero.");
        }
    }
}
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

namespace Auditt.Application.Features.Scales;

public class CreateScale : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/scales", async (IMediator mediator, CreateScaleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateScale))
        .WithTags(nameof(Scale))
        .ProducesValidationProblem()
        .Produces<CreateScaleResponse>(StatusCodes.Status200OK);
    }
    public record CreateScaleCommand : IRequest<IResult>
    {
        public string Name { get; init; } = string.Empty;
    }
    public record CreateScaleResponse(int Id, string Name);

    public class CreateScaleHandler(AppDbContext context, IValidator<CreateScaleCommand> validator) : IRequestHandler<CreateScaleCommand, IResult>
    {
        public async Task<IResult> Handle(CreateScaleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var scale = Scale.Create(0, request.Name);
            await context.Scales.AddAsync(scale);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateScaleResponse(scale.Id, scale.Name);
                return Results.Ok(Result<Scale>.Success(scale, "Escala creada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al crear la escala")));
            }
        }
    }
    public class CreateScaleValidator : AbstractValidator<CreateScaleCommand>
    {
        public CreateScaleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("El nombre no puede estar vacío.");
        }
    }
}
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

public class UpdateScale : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/scales", async (IMediator mediator, UpdateScaleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateScale))
        .WithTags(nameof(Scale))
        .ProducesValidationProblem()
        .Produces<UpdateScaleResponse>(StatusCodes.Status200OK);
    }
    public record UpdateScaleCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
    public record UpdateScaleResponse(int Id, string Name);
    public class UpdateScaleHandler(AppDbContext context, IValidator<UpdateScaleCommand> validator) : IRequestHandler<UpdateScaleCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateScaleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var scale = await context.Scales.FindAsync(request.Id);
            if (scale == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Escala no encontrada")));
            }
            scale.Update(request.Name);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdateScaleResponse(scale.Id, scale.Name);
                return Results.Ok(Result<Scale>.Success(scale, "Escala actualizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdatePaciente", "Error al actualizar la escala")));
            }
        }
    }
    public class UpdateScaleValidator : AbstractValidator<UpdateScaleCommand>
    {
        public UpdateScaleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la escala es requerido")
                .MaximumLength(100).WithMessage("El nombre de la escala no puede exceder los 100 caracteres");
        }
    }
}
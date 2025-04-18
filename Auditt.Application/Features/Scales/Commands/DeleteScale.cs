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

public class DeleteScale : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/scales/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteScaleCommand(id));
        })
        .WithName(nameof(DeleteScale))
        .WithTags(nameof(Scale))
        .ProducesValidationProblem()
        .Produces<DeleteScaleResponse>(StatusCodes.Status200OK);
    }
    public record DeleteScaleCommand(int Id) : IRequest<IResult>;
    public record DeleteScaleResponse(int Id, string Name);
    public class DeleteScaleHandler(AppDbContext context, IValidator<DeleteScaleCommand> validator) : IRequestHandler<DeleteScaleCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteScaleCommand request, CancellationToken cancellationToken)
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
            context.Scales.Remove(scale);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeleteScaleResponse(scale.Id, scale.Name);
                return Results.Ok(Result<Scale>.Success(scale, "Escala eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDeletePaciente", "Error al eliminar la escala")));
            }
        }
    }
    public class DeleteScaleValidator : AbstractValidator<DeleteScaleCommand>
    {
        public DeleteScaleValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID de la escala debe ser mayor que cero.");
        }
    }
}
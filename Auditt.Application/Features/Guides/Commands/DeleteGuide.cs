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

namespace Auditt.Application.Features.Questions;
public class DeleteGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/guides/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteGuideCommand(id));
        })
        .WithName(nameof(DeleteGuide))
        .WithTags(nameof(Guide))
        .ProducesValidationProblem()
        .Produces<DeleteGuideResponse>(StatusCodes.Status200OK);
    }
    public record DeleteGuideCommand(int Id) : IRequest<IResult>;
    public record DeleteGuideResponse(int Id);
    public class DeleteGuideHandler(AppDbContext context, IValidator<DeleteGuideCommand> validator) : IRequestHandler<DeleteGuideCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteGuideCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var guide = await context.Guides.FindAsync(request.Id);
            if (guide == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Guía no encontrada")));
            }
            context.Guides.Remove(guide);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeleteGuideResponse(guide.Id);
                return Results.Ok(Result<DeleteGuideResponse>.Success(resModel, "Guía eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDelete", "Error al eliminar la guía")));
            }
        }
    }
    public class DeleteGuideValidator : AbstractValidator<DeleteGuideCommand>
    {
        public DeleteGuideValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la guía debe ser mayor que 0");
        }
    }
}
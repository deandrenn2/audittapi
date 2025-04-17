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
public class UpdateGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/guides", async (IMediator mediator, UpdateGuideCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateGuide))
        .WithTags(nameof(Guide))
        .ProducesValidationProblem()
        .Produces<UpdateGuideResponse>(StatusCodes.Status200OK);
    }
    public record UpdateGuideCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int IdScale { get; init; }
    }
    public record UpdateGuideResponse(int Id, string Name, string Description, int IdScale);
    public class UpdateGuideHandler(AppDbContext context, IValidator<UpdateGuideCommand> validator) : IRequestHandler<UpdateGuideCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateGuideCommand request, CancellationToken cancellationToken)
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
            guide.Update(request.Name, request.Description, request.IdScale);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdateGuideResponse(guide.Id, guide.Name, guide.Description, guide.IdScale);
                return Results.Ok(Result<UpdateGuideResponse>.Success(resModel, "Guía actualizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdate", "Error al actualizar la guía")));
            }
        }
    }
    public class UpdateGuideValidator : AbstractValidator<UpdateGuideCommand>
    {
        public UpdateGuideValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");
            RuleFor(x => x.IdScale)
                .GreaterThan(0).WithMessage("La escala es obligatoria");
        }
    }
}
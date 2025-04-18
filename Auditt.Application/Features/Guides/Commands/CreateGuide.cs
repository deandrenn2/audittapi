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

namespace Auditt.Application.Features.Guides;
public class CreateGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/guides", async (IMediator mediator, CreateGuideCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateGuide))
        .WithTags(nameof(Guide))
        .ProducesValidationProblem()
        .Produces<CreateGuideResponse>(StatusCodes.Status200OK);
    }

    public record CreateGuideCommand : IRequest<IResult>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int IdScale { get; init; }
    }
    public record CreateGuideResponse(int Id, string Name, string Description, int IdScale);
    public class CreateGuideHandler(AppDbContext context, IValidator<CreateGuideCommand> validator) : IRequestHandler<CreateGuideCommand, IResult>
    {
        public async Task<IResult> Handle(CreateGuideCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var guide = Guide.Create(0, request.Name, request.Description, request.IdScale);
            await context.Guides.AddAsync(guide);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateGuideResponse(guide.Id, guide.Name, guide.Description, guide.IdScale);
                return Results.Ok(Result<Guide>.Success(guide, "Guía creada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al crear la guía")));
            }
        }
    }
    public class CreateGuideValidator : AbstractValidator<CreateGuideCommand>
    {
        public CreateGuideValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres");
            RuleFor(x => x.IdScale)
                .GreaterThan(0).WithMessage("El id de la escala debe ser mayor que 0");
        }
    }
}
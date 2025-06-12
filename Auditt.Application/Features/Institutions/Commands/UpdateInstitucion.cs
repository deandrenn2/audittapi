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

public class UpdateInstitucion : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/institutions", async (HttpRequest req, IMediator mediator, UpdateInstitucionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateInstitucion))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<UpdateInstitucionResponse>(StatusCodes.Status200OK);
    }
    public record UpdateInstitucionCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Abbreviation { get; init; } = string.Empty;
        public string Nit { get; init; } = string.Empty;
        public string City { get; init; } = string.Empty;
        public string Manager { get; init; } = string.Empty;
        public string AssistantManager { get; init; } = string.Empty;
    }
    public record UpdateInstitucionResponse(string Name, string Abbreviation, string Nit, string City);

    public class UpdateInstitucionHandler(AppDbContext context, IValidator<UpdateInstitucionCommand> validator) : IRequestHandler<UpdateInstitucionCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateInstitucionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<Dictionary<string, string[]>>.Failure(
                result.GetValidationProblems(),
                new Error("Intitution.ErrorValidation", "Se presentaron errores de validación")
            ));
            }

            var institucion = await context.Institutions.FindAsync(request.Id);
            if (institucion == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdateInstitucion", "Error al actualizar la institución")));
            }
            institucion.Update(request.Name, request.Abbreviation, request.Nit, request.City, request.Manager, request.AssistantManager);
            var resCount = await context.SaveChangesAsync(cancellationToken);
            if (resCount > 0)
            {
                var resModel = new UpdateInstitucionResponse(institucion.Name, institucion.Abbreviation, institucion.Nit, institucion.City);
                return Results.Ok(Result<UpdateInstitucionResponse>.Success(resModel, "Institución actualizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdateInstitucion", "Error al actualizar la institución")));
            }
        }

    }

    public class UpdateInstitucionValidator : AbstractValidator<UpdateInstitucionCommand>
    {
        public UpdateInstitucionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la institución es requerido")
                .MaximumLength(100).WithMessage("El nombre de la institución no puede exceder los 100 caracteres");
            RuleFor(x => x.Abbreviation)
                .NotEmpty().WithMessage("La abreviatura de la institución es requerida");
            RuleFor(x => x.Nit)
                .NotEmpty().WithMessage("El NIT de la institución es requerido")
                .MaximumLength(20).WithMessage("El NIT de la institución no puede exceder los 20 caracteres");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad de la institución es requerida")
                .MaximumLength(50).WithMessage("La ciudad de la institución no puede exceder los 50 caracteres");
        }
    }
}
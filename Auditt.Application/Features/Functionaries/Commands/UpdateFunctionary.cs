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

namespace Auditt.Application.Features.Functionaries;

public class UpdateFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/functionaries/{id:int}", async (HttpRequest req, IMediator mediator, int id, UpdateFunctionaryCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateFunctionary))
        .WithTags(nameof(Functionary))
        .ProducesValidationProblem()
        .Produces<UpdateFunctionaryResponse>(StatusCodes.Status200OK);
    }
    public record UpdateFunctionaryCommand : IRequest<IResult>
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Identification { get; init; } = string.Empty;
    }

    public record UpdateFunctionaryResponse(string FirstName, string LastName, string Identification);
    public class UpdateFunctionaryHandler(AppDbContext context, IValidator<UpdateFunctionaryCommand> validator) : IRequestHandler<UpdateFunctionaryCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateFunctionaryCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var functionary = await context.Functionaries.FindAsync(request.Id);
            if (functionary == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Funcionario no encontrado")));
            }
            functionary.Update(request.FirstName, request.LastName, request.Identification);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdateFunctionaryResponse(functionary.FirstName, functionary.LastName, functionary.Identification);
                return Results.Ok(Result<Functionary>.Success(functionary, "Funcionario actualizado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdateInstitucion", "Error al actualizar el funcionario")));
            }
        }
    }
    public class UpdateFunctionaryValidator : AbstractValidator<UpdateFunctionaryCommand>
    {
        public UpdateFunctionaryValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("El nombre es obligatorio");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("El apellido es obligatorio");
            RuleFor(x => x.Identification)
                .NotEmpty()
                .WithMessage("La identificación es obligatoria");
        }
    }
}
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

public class CreateFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/functionaries", async (HttpRequest req, IMediator mediator, CreateFunctionaryCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateFunctionary))
        .WithTags(nameof(Functionary))
        .ProducesValidationProblem()
        .Produces<CreateFunctionaryResponse>(StatusCodes.Status201Created);
    }
    public record CreateFunctionaryCommand : IRequest<IResult>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Identification { get; init; } = string.Empty;
    }
    public record CreateFunctionaryResponse(string FirstName, string LastName, string Identification);
    public class CreateFunctionaryHandler(AppDbContext context, IValidator<CreateFunctionaryCommand> validator) : IRequestHandler<CreateFunctionaryCommand, IResult>
    {
        public async Task<IResult> Handle(CreateFunctionaryCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var newFunctionary = Functionary.Create(0, request.FirstName, request.LastName, request.Identification);
            context.Add(newFunctionary);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateFunctionaryResponse(newFunctionary.FirstName, newFunctionary.LastName, newFunctionary.Identification);
                return Results.Ok(Result<Functionary>.Success(newFunctionary, "Funcionario creado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreateInstitucion", "Error al crear el funcionario")));
            }
        }

    }
    public class CreateFunctionaryValidator : AbstractValidator<CreateFunctionaryCommand>
    {
        public CreateFunctionaryValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Los nombres son requeridos");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Los apellidos son requeridos");
            RuleFor(x => x.Identification)
                .NotEmpty()
                .WithMessage("El número de identificación es requerido");
        }
    }
}
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

public class DeleteFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/functionaries/{id:int}", async (HttpRequest req, IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteFunctionaryCommand(id));
        })
        .WithName(nameof(DeleteFunctionary))
        .WithTags(nameof(Functionary))
        .ProducesValidationProblem()
        .Produces<DeleteFunctionaryResponse>(StatusCodes.Status200OK);
    }
    public record DeleteFunctionaryCommand(int Id) : IRequest<IResult>;
    public record DeleteFunctionaryResponse(string Message);
    public class DeleteFunctionaryHandler(AppDbContext context, IValidator<DeleteFunctionaryCommand> validator) : IRequestHandler<DeleteFunctionaryCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteFunctionaryCommand request, CancellationToken cancellationToken)
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
            context.Functionaries.Remove(functionary);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                return Results.Ok(Result<DeleteFunctionaryResponse>.Success(new DeleteFunctionaryResponse("Funcionario eliminado correctamente"), "Funcionario eliminado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDeleteInstitucion", "Error al eliminar el funcionario")));
            }
        }
    }
    public class DeleteFunctionaryValidator : AbstractValidator<DeleteFunctionaryCommand>
    {
        public DeleteFunctionaryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID del funcionario debe ser mayor que 0");
        }
    }
}
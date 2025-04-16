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

public class DeleteInstitution : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/institutions/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteInstitutionCommand(id));
        })
        .WithName(nameof(DeleteInstitution))
        .WithTags(nameof(Institution))
        .ProducesValidationProblem()
        .Produces<int>(StatusCodes.Status200OK);
    }
    public record DeleteInstitutionCommand(int Id) : IRequest<IResult>;
    public record DeleteInstitutionResponse(string Message);
    public class DeleteInstitutionHandler(AppDbContext context) : IRequestHandler<DeleteInstitutionCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
        {
            var institution = await context.Institutions.FindAsync(request.Id);
            if (institution == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDeleteInstitucion", "Error al eliminar la institución")));
            }
            context.Institutions.Remove(institution);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                return Results.Ok(Result<int>.Success(request.Id, "Institución eliminada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDeleteInstitucion", "Error al eliminar la institución")));
            }
        }
    }
    public class DeleteInstitutionValidator : AbstractValidator<DeleteInstitutionCommand>
    {
        public DeleteInstitutionValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id de la institución es requerido");
        }
    }
}

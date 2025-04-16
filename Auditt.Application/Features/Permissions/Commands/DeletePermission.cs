using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auditt.Application.Features.Permissions;

public class DeletePermission : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/permissions/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeletePermissionCommand(id));
        })
        .WithName(nameof(DeletePermission))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<DeletePermissionResponse>(StatusCodes.Status200OK);
    }
    public record DeletePermissionCommand(int Id) : IRequest<IResult>;
    public record DeletePermissionResponse(int Id, string Name, string Description);
    public class DeletePermissionHandler(AppDbContext context, IValidator<DeletePermissionCommand> validator) : IRequestHandler<DeletePermissionCommand, IResult>
    {
        public async Task<IResult> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var permission = await context.Permissions.FindAsync(new object[] { request.Id }, cancellationToken);
            if (permission == null)
            {
                return Results.NotFound(new { Message = "Permiso no encontrado" });
            }
            context.Permissions.Remove(permission);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new DeletePermissionResponse(permission.Id, permission.Name, permission.Description);
                return Results.Ok(Result<DeletePermissionResponse>.Success(resModel, "Permiso eliminado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorDeleteRole", "Error al eliminar el permiso")));
            }
        }
    }

    public class DeletePermissionValidator : AbstractValidator<DeletePermissionCommand>
    {
        public DeletePermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
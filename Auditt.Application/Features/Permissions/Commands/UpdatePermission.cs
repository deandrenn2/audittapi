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

public class UpdatePermission : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/permissions", async (IMediator mediator, UpdatePermissionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdatePermission))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<UpdatePermissionResponse>(StatusCodes.Status200OK);
    }
    public record UpdatePermissionCommand(int Id, string Name, string Code, string? Description) : IRequest<IResult>;
    public record UpdatePermissionResponse(int Id, string Name, string Code, string? Description);

    public class UpdatePermissionHandler(AppDbContext context, IValidator<UpdatePermissionCommand> validator) : IRequestHandler<UpdatePermissionCommand, IResult>
    {
        public async Task<IResult> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
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
            permission.Update(request.Name, request.Code, request?.Description);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdatePermissionResponse(permission.Id, permission.Name, permission.Code, permission.Description ?? string.Empty);
                return Results.Ok(Result<UpdatePermissionResponse>.Success(resModel, "Permiso actualizado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdateRole", "Error al actualizar el permiso")));
            }
        }
    }
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
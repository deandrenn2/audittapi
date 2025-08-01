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
using Auditt.Application.Infrastructure.Authorization;

namespace Auditt.Application.Features.Roles;

public class UpdateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/roles", async (IMediator mediator, UpdateRoleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateRole))
        .WithTags(nameof(Role))
        .RequireAdmin() // Solo ADMIN puede actualizar roles
        .ProducesValidationProblem()
        .Produces<UpdateRoleResponse>(StatusCodes.Status200OK);
    }
    public record UpdateRoleCommand(int Id, string Name, string Description) : IRequest<IResult>;
    public record UpdateRoleResponse(int Id, string Name, string Description);

    public class UpdateRoleHandler(AppDbContext context, IValidator<UpdateRoleCommand> validator) : IRequestHandler<UpdateRoleCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var role = await context.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
            if (role == null)
            {
                return Results.NotFound(new { Message = "Rol no encontrado" });
            }
            role.Update(request.Name, request.Description);
            context.Roles.Update(role);
            var resCount = await context.SaveChangesAsync(cancellationToken);
            if (resCount > 0)
            {
                var resModel = new UpdateRoleResponse(role.Id, role.Name, role.Description);
                return Results.Ok(Result<UpdateRoleResponse>.Success(resModel, "Rol actualizado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdateRole", "Error al actualizar el rol")));
            }
        }
    }
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
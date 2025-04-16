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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Features.Permissions;

public class AddPermissionRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/roles/{id:int}/permissions", async (IMediator mediator, int id, AddPermissionRoleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(AddPermissionRole))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<Result>(StatusCodes.Status200OK);
    }
    public record AddPermissionRoleCommand(int Id, List<int> Permissions) : IRequest<IResult>;
    public record PermisionModel(int Id, string Name, string Description);
    public class AddPermissionRoleHandler(AppDbContext context, IValidator<AddPermissionRoleCommand> validator) : IRequestHandler<AddPermissionRoleCommand, IResult>
    {
        public async Task<IResult> Handle(AddPermissionRoleCommand request, CancellationToken cancellationToken)
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
            var permissions = await context.Permissions.Where(x => request.Permissions.Contains(x.Id)).ToListAsync(cancellationToken);
            if (permissions.Count == 0)
            {
                return Results.NotFound(new { Message = "Permisos no encontrados" });
            }
            foreach (var permission in permissions)
            {
                if (!role.Permissions.Contains(permission))
                {
                    role.Permissions.Add(permission);
                }
            }
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new List<PermisionModel>();
                foreach (var permission in role.Permissions)
                {
                    resModel.Add(new PermisionModel(permission.Id, permission.Name, permission.Description));
                }
                return Results.Ok(Result<List<PermisionModel>>.Success(resModel, "Permisos agregados correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorAddRole", "Error al agregar los permisos")));
            }
        }
    }
    public class AddPermissionRoleValidator : AbstractValidator<AddPermissionRoleCommand>
    {
        public AddPermissionRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
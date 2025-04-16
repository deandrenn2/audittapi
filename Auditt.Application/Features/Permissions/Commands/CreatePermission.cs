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

public class CreatePermission : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/permissions", async (IMediator mediator, CreatePermissionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreatePermission))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<CreatePermissionResponse>(StatusCodes.Status200OK);
    }
    public record CreatePermissionCommand(string Name, string Description) : IRequest<IResult>;
    public record CreatePermissionResponse(int Id, string Name, string Description);
    public class CreatePermissionHandler(AppDbContext context, IValidator<CreatePermissionCommand> validator) : IRequestHandler<CreatePermissionCommand, IResult>
    {
        public async Task<IResult> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var permission = Permission.Create(0, request.Name, request.Description);
            await context.Permissions.AddAsync(permission);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreatePermissionResponse(permission.Id, permission.Name, permission.Description);
                return Results.Ok(Result<CreatePermissionResponse>.Success(resModel, "Permiso creado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreateRole", "Error al crear el permiso")));
            }
        }
    }
    public class CreatePermissionValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
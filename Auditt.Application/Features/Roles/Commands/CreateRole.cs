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
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Infrastructure.Authorization;

namespace Auditt.Application.Features.Roles;

public class CreateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/roles", async (IMediator mediator, CreateRoleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateRole))
        .WithTags(nameof(Role))
        .RequireAdmin() // Solo ADMIN puede crear roles
        .ProducesValidationProblem()
        .Produces<CreateRoleResponse>(StatusCodes.Status200OK);
    }
    public record CreateRoleCommand(string Name, string Description) : IRequest<IResult>;
    public record CreateRoleResponse(int Id, string Name, string Description);
    public class CreateRoleHandler(AppDbContext context, IValidator<CreateRoleCommand> validator) : IRequestHandler<CreateRoleCommand, IResult>
    {
        public async Task<IResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var idRolNext = 1;
            var idRol = await context.Roles.MaxAsync(x => x.Id, cancellationToken);
            idRolNext = idRol + 1;

            var role = Role.Create(idRolNext, request.Name, request.Description);
            await context.Roles.AddAsync(role);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateRoleResponse(role.Id, role.Name, role.Description);
                return Results.Ok(Result<CreateRoleResponse>.Success(resModel, "Rol creado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreateRole", "Error al crear el rol")));
            }
        }
    }

    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}
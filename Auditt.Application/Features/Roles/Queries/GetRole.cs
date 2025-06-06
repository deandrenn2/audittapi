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

namespace Auditt.Application.Features.Roles;

public class GetRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/roles/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetRoleCommand(id));
        })
        .WithName(nameof(GetRole))
        .WithTags(nameof(Role))
        .ProducesValidationProblem()
        .Produces<GetRoleResponse>(StatusCodes.Status200OK);
    }
    public record GetRoleCommand(int Id) : IRequest<IResult>;
    public record GetRoleResponse(int Id, string Name, string Description, List<PermissionModel> Permissions);
    public record PermissionModel(int Id, string Name, string Code, string? Description);
    public class GetRoleHandler(AppDbContext context, IValidator<GetRoleCommand> validator) : IRequestHandler<GetRoleCommand, IResult>
    {
        public async Task<IResult> Handle(GetRoleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var role = await context.Roles.Include(x => x.Permissions).FirstAsync(x => x.Id == request.Id, cancellationToken);
            if (role == null)
            {
                return Results.NotFound(new { Message = "Rol no encontrado" });
            }
            var resModel = new GetRoleResponse(role.Id, role.Name, role.Description,
            [.. role.Permissions.Select(permission => new PermissionModel(permission.Id, permission.Name, permission.Code, permission.Description))]);
            return Results.Ok(Result<GetRoleResponse>.Success(resModel, "Rol encontrado correctamente"));
        }
    }
    public class GetRoleValidator : AbstractValidator<GetRoleCommand>
    {
        public GetRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
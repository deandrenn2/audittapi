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

public class DeleteRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/roles/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteRoleCommand(id));
        })
        .WithName(nameof(DeleteRole))
        .WithTags(nameof(Role))
        .RequireAdmin() // Solo ADMIN puede eliminar roles
        .ProducesValidationProblem()
        .Produces<DeleteRoleResponse>(StatusCodes.Status200OK);
    }
    public record DeleteRoleCommand(int Id) : IRequest<IResult>;
    public record DeleteRoleResponse(int Id);
    public class DeleteRoleHandler(AppDbContext _dbContext, IValidator<DeleteRoleCommand> validator) : IRequestHandler<DeleteRoleCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var role = await _dbContext.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
            if (role == null)
            {
                return Results.NotFound(new { Message = "Rol no encontrado" });
            }
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok(Result<DeleteRoleResponse>.Success(new DeleteRoleResponse(role.Id), "Rol eliminado correctamente"));
        }
    }
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
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

public class GetPermission : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/permissions/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetPermissionCommand(id));
        })
        .WithName(nameof(GetPermission))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<GetPermissionResponse>(StatusCodes.Status200OK);
    }
    public record GetPermissionCommand(int Id) : IRequest<IResult>;
    public record GetPermissionResponse(int Id, string Name, string Code, string Description);
    public class GetPermissionHandler(AppDbContext context, IValidator<GetPermissionCommand> validator) : IRequestHandler<GetPermissionCommand, IResult>
    {
        public async Task<IResult> Handle(GetPermissionCommand request, CancellationToken cancellationToken)
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
            var resModel = new GetPermissionResponse(permission.Id, permission.Name, permission.Code, permission.Description ?? string.Empty);
            return Results.Ok(Result<GetPermissionResponse>.Success(resModel, "Permiso encontrado correctamente"));
        }
    }
    public class GetPermissionValidator : AbstractValidator<GetPermissionCommand>
    {
        public GetPermissionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
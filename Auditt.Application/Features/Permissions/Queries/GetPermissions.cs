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

namespace Auditt.Application.Features.Permissions;

public class GetPermissions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/permissions", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetPermissionsCommand());
        })
        .WithName(nameof(GetPermissions))
        .WithTags(nameof(Permission))
        .ProducesValidationProblem()
        .Produces<GetPermissionsResponse>(StatusCodes.Status200OK);
    }
    public record GetPermissionsCommand() : IRequest<IResult>;
    public record GetPermissionsResponse(int Id, string Name, string Description);
    public class GetPermissionsHandler(AppDbContext context, IValidator<GetPermissionsCommand> validator) : IRequestHandler<GetPermissionsCommand, IResult>
    {
        public async Task<IResult> Handle(GetPermissionsCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var permissions = await context.Permissions.ToListAsync(cancellationToken);
            var resModel = permissions.Select(p => new GetPermissionsResponse(p.Id, p.Name, p.Description)).ToList();
            return Results.Ok(Result<List<GetPermissionsResponse>>.Success(resModel, "Permisos encontrados correctamente"));
        }
    }

    public class GetPermissionsValidator : AbstractValidator<GetPermissionsCommand>
    {
        public GetPermissionsValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Request cannot be null");
        }
    }

}

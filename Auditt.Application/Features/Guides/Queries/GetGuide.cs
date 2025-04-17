using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;

namespace Auditt.Application.Features.Guides;
public class GetGuide : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/guides/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetGuideCommand(id));
        })
        .WithName(nameof(GetGuide))
        .WithTags(nameof(Guide))
        .ProducesValidationProblem()
        .Produces<GetGuideResponse>(StatusCodes.Status200OK);
    }
    public record GetGuideCommand(int Id) : IRequest<IResult>;
    public record GetGuideResponse(int Id, string Name, string Description, int IdScale);
    public class GetGuideHandler(AppDbContext context, IValidator<GetGuideCommand> validator) : IRequestHandler<GetGuideCommand, IResult>
    {
        public async Task<IResult> Handle(GetGuideCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var guide = await context.Guides.FindAsync(request.Id);
            if (guide == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Guía no encontrada")));
            }
            var resModel = new GetGuideResponse(guide.Id, guide.Name, guide.Description, guide.IdScale);
            return Results.Ok(Result<GetGuideResponse>.Success(resModel, "Guía obtenida correctamente"));
        }
    }
    public class GetGuideValidator : AbstractValidator<GetGuideCommand>
    {
        public GetGuideValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El Id de la guía debe ser mayor que 0");
        }
    }
}
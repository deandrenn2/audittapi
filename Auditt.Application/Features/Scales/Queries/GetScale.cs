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
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Features.Scales;

public class GetScale : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/scales/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetScaleQuery(id));
        })
        .WithName(nameof(GetScale))
        .WithTags(nameof(Scale))
        .ProducesValidationProblem()
        .Produces<GetScaleResponseModel>(StatusCodes.Status200OK);
    }
    public record GetScaleQuery(int Id) : IRequest<IResult>;
    public record GetScaleResponseModel(int Id, string Name, List<EquivalenceModel> Equivalences);
    public record EquivalenceModel(int Id, string Name, decimal Value);
    public class GetScaleHandler(AppDbContext context, IValidator<GetScaleQuery> validator) : IRequestHandler<GetScaleQuery, IResult>
    {
        public async Task<IResult> Handle(GetScaleQuery request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var scale = await context.Scales.Include(x => x.Equivalences).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (scale == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Escala no encontrada")));
            }

            var equivalences = scale.Equivalences.Select(x => new EquivalenceModel(x.Id, x.Name, x.Value)).ToList();

            var resModel = new GetScaleResponseModel(scale.Id, scale.Name, equivalences);
            return Results.Ok(Result<GetScaleResponseModel>.Success(resModel, "Escala obtenida correctamente"));
        }
    }
    public class GetScaleValidator : AbstractValidator<GetScaleQuery>
    {
        public GetScaleValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("El ID de la escala debe ser mayor que cero.");
        }
    }

}
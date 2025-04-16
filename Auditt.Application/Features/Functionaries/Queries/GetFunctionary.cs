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

namespace Auditt.Application.Features.Functionaries;

public class GetFunctionary : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/functionaries/{id}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetFunctionaryQuery(id));
        }).Accepts<GetFunctionaryQuery>("application/json")
          .WithName(nameof(GetFunctionary))
          .WithTags(nameof(Functionary))
          .ProducesValidationProblem()
          .Produces<GetFunctionaryResponse>(StatusCodes.Status200OK);
    }

    public record GetFunctionaryQuery(int Id) : IRequest<IResult>;
    public record GetFunctionaryResponse(int Id, string FirstName, string LastName, string Identification);
    public class GetFunctionaryHandler(AppDbContext context) : IRequestHandler<GetFunctionaryQuery, IResult>
    {
        public async Task<IResult> Handle(GetFunctionaryQuery request, CancellationToken cancellationToken)
        {
            var functionary = await context.Functionaries.FindAsync(request.Id);
            if (functionary == null)
            {
                return Results.NotFound(Result.Failure(new Error("Functionary.NotFound", "Functionary not found")));
            }
            var resModel = new GetFunctionaryResponse(functionary.Id, functionary.FirstName, functionary.LastName, functionary.Identification);
            return Results.Ok(Result<GetFunctionaryResponse>.Success(resModel, "Functionary retrieved successfully"));
        }
    }

}

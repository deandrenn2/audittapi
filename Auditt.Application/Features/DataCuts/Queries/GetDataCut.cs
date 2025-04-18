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

namespace Auditt.Application.Features.DataCuts;

public class GetDataCut : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/datacut/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetDataCutQuery(id));
        })
        .WithName(nameof(GetDataCut))
        .WithTags(nameof(DataCut))
        .ProducesValidationProblem()
        .Produces<GetDataCutResponse>(StatusCodes.Status200OK);
    }
    public record GetDataCutQuery(int Id) : IRequest<IResult>;
    public record GetDataCutResponse(string Name, string Cycle, DateTime InitialDate, DateTime FinalDate, int MaxHistory, int InstitutionId);
    public class GetDataCutHandler(AppDbContext context) : IRequestHandler<GetDataCutQuery, IResult>
    {
        public async Task<IResult> Handle(GetDataCutQuery request, CancellationToken cancellationToken)
        {
            var dataCut = await context.DataCuts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (dataCut == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Corte no encontrado")));
            }
            var response = new GetDataCutResponse(dataCut.Name, dataCut.Cycle, dataCut.InitialDate, dataCut.FinalDate, dataCut.MaxHistory, dataCut.InstitutionId);
            return Results.Ok(Result<GetDataCutResponse>.Success(response,"Lista de cortes de datos"));
        }
    }
    public class GetDataCutValidator : AbstractValidator<GetDataCutQuery>
    {
        public GetDataCutValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El id del corte es requerido");
        }
    }
}
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

public class DeleteDataCut : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/datacut/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeleteDataCutCommand(id));
        })
        .WithName(nameof(DeleteDataCut))
        .WithTags(nameof(DataCut))
        .ProducesValidationProblem()
        .Produces<DeleteDataCutResponse>(StatusCodes.Status200OK);
    }
    public record DeleteDataCutCommand(int Id) : IRequest<IResult>;
    public record DeleteDataCutResponse(string Message);
    public class DeleteDataCutHandler(AppDbContext context) : IRequestHandler<DeleteDataCutCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteDataCutCommand request, CancellationToken cancellationToken)
        {
            var dataCut = await context.DataCuts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (dataCut == null)
            {
                return Results.NotFound(Result.Failure(new Error("Login.ErrorNotFound", "Corte no encontrado")));
            }
            context.DataCuts.Remove(dataCut);
            await context.SaveChangesAsync(cancellationToken);
            return Results.Ok(Result<string>.Success("Corte eliminado correctamente"));
        }
    }
    public class DeleteDataCutValidator : AbstractValidator<DeleteDataCutCommand>
    {
        public DeleteDataCutValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El id del corte es requerido");
        }
    }
}
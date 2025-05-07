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

public class UpdateDataCut : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/datacut/{id:int}", async (HttpRequest req, IMediator mediator, int id, UpdateDataCutCommand command) =>
        {
            return await mediator.Send(command with { Id = id });
        })
        .WithName(nameof(UpdateDataCut))
        .WithTags(nameof(DataCut))
        .ProducesValidationProblem()
        .Produces<UpdateDataCutResponse>(StatusCodes.Status200OK);
    }
    public record UpdateDataCutCommand(int Id, string Name, string Cycle, DateTime InitialDate, DateTime FinalDate, int MaxHistory) : IRequest<IResult>;
    public record UpdateDataCutResponse(int Id, string Name, string Cycle, DateTime InitialDate, DateTime FinalDate, int MaxHistory);

    public class UpdateDataCutCommandHandler(AppDbContext _context) : IRequestHandler<UpdateDataCutCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateDataCutCommand request, CancellationToken cancellationToken)
        {
            var dataCut = await _context.DataCuts.FindAsync(new object[] { request.Id }, cancellationToken);
            if (dataCut == null)
            {
                return Results.NotFound();
            }
            dataCut.Update(request.Name, request.Cycle, request.InitialDate, request.FinalDate, request.MaxHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return Results.Ok(Result<UpdateDataCutResponse>.Success(new UpdateDataCutResponse(dataCut.Id, dataCut.Name, dataCut.Cycle, dataCut.InitialDate, dataCut.FinalDate, dataCut.MaxHistory), "Actualizado correctamente"));
        }
    }

    public class UpdateDataCutCommandValidator : AbstractValidator<UpdateDataCutCommand>
    {
        public UpdateDataCutCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("El nombre no puede estar vacío.");
            RuleFor(x => x.Cycle)
                .NotEmpty()
                .WithMessage("El ciclo no puede estar vacío.");
            RuleFor(x => x.InitialDate)
                .NotEmpty()
                .WithMessage("La fecha inicial no puede estar vacía.");
            RuleFor(x => x.FinalDate)
                .NotEmpty()
                .WithMessage("La fecha final no puede estar vacía.");
            RuleFor(x => x.MaxHistory)
                .GreaterThan(0)
                .WithMessage("El máximo de historia debe ser mayor a 0.");
        }
    }
}

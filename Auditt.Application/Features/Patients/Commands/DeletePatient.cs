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

namespace Auditt.Application.Features.Patients;

public class DeletePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/patients/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new DeletePatientCommand(id));
        })
        .WithName(nameof(DeletePatient))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<DeletePatientResponse>(StatusCodes.Status200OK);
    }
    public record DeletePatientCommand(int Id) : IRequest<IResult>;
    public record DeletePatientResponse(string Message);

    public class DeletePatientHandler(AppDbContext _dbContext, IValidator<DeletePatientCommand> validator) : IRequestHandler<DeletePatientCommand, IResult>
    {
        public async Task<IResult> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }

            var patient = await _dbContext.Patients.FindAsync(new object[] { request.Id }, cancellationToken);
            if (patient == null)
            {
                return Results.NotFound(new { Message = "Paciente no encontrado" });
            }
            _dbContext.Patients.Remove(patient);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok(new DeletePatientResponse("Paciente eliminado correctamente"));
        }
    }

    public class DeletePatientValidator : AbstractValidator<DeletePatientCommand>
    {
        public DeletePatientValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
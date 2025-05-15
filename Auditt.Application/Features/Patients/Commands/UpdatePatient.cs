using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Carter;

public class UpdatePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/patients/{id:int}", async (IMediator mediator, int id, UpdatePatientCommand command) =>
        {
            return await mediator.Send(new UpdatePatientCommand(id, command.FirstName, command.LastName, command.Identification, command.BirthDate, command.Eps));
        })
        .WithName(nameof(UpdatePatient))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<UpdatePatientResponse>(StatusCodes.Status200OK);
    }
    public record UpdatePatientCommand(int Id, string FirstName, string LastName, string Identification, DateTime BirthDate, string Eps) : IRequest<IResult>;
    public record UpdatePatientResponse(string Message);

    public class UpdatePatientHandler(AppDbContext context, IValidator<UpdatePatientCommand> validator) : IRequestHandler<UpdatePatientCommand, IResult>
    {
        public async Task<IResult> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }
            var patient = await context.Patients.FindAsync(request.Id);
            if (patient == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdatePaciente", "Error al actualizar el paciente")));
            }
            patient.Update(request.FirstName, request.LastName, request.Identification, request.BirthDate, request.Eps);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new UpdatePatientResponse("Paciente actualizado correctamente");
                return Results.Ok(Result<Patient>.Success(patient, "Paciente actualizado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorUpdatePaciente", "Error al actualizar el paciente")));
            }
        }
    }

    public class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientValidator()
        {
            RuleFor(x => x.Identification).NotEmpty().WithMessage("El número de documento es requerido");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("La fecha de nacimiento es requerida");
            RuleFor(x => x.Eps).NotEmpty().WithMessage("La EPS es requerida");
        }
    }
}
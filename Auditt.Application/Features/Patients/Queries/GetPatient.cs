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

namespace Auditt.Application.Features.Patients;

public class GetPatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/patients/{id:int}", async (IMediator mediator, int id) =>
        {
            return await mediator.Send(new GetPatientQuery(id));
        })
        .WithName(nameof(GetPatient))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<GetPatientResponse>(StatusCodes.Status200OK);
    }
    public record GetPatientQuery(int Id) : IRequest<IResult>;
    public record GetPatientResponse(string FirstName, string LastName, string DocumentNumber, DateTime BirthDate, string Eps);
    public class GetPatientHandler(AppDbContext context) : IRequestHandler<GetPatientQuery, IResult>
    {
        public async Task<IResult> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var patient = await context.Patients.FindAsync(request.Id);
            if (patient == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorGetPaciente", "Error al obtener el paciente")));
            }
            var resModel = new GetPatientResponse(patient.FirstName, patient.LastName, patient.Identification, patient.BirthDate, patient.Eps);
            return Results.Ok(Result<Patient>.Success(patient, "Paciente obtenido correctamente"));
        }
    }
    public class GetPatientValidator : AbstractValidator<GetPatientQuery>
    {
        public GetPatientValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("El id del paciente es requerido");
        }
    }
}
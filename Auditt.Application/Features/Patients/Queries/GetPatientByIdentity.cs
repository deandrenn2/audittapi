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
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Features.Patients;

public class GetPatientByDocument : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/patients/{identity}", async (IMediator mediator, string identity) =>
        {
            return await mediator.Send(new GetPatientQuery(identity));
        })
        .WithName(nameof(GetPatientByDocument))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<GetPatientResponse>(StatusCodes.Status200OK);
    }
    public record GetPatientQuery(string identity) : IRequest<IResult>;
    public record GetPatientResponse(int Id, string FirstName, string LastName, string Identification, DateTime BirthDate, string Eps);
    public class GetPatientHandler(AppDbContext context) : IRequestHandler<GetPatientQuery, IResult>
    {
        public async Task<IResult> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var patient = await context.Patients.Where(x => x.Identification == request.identity).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorGetPaciente", "Error al obtener el paciente")));
            }
            var resModel = new GetPatientResponse(patient.Id, patient.FirstName, patient.LastName, patient.Identification, patient.BirthDate, patient.Eps);
            return Results.Ok(Result<Patient>.Success(patient, "Paciente obtenido correctamente"));
        }
    }
    public class GetPatientValidator : AbstractValidator<GetPatientQuery>
    {
        public GetPatientValidator()
        {
            RuleFor(x => x.identity).NotEmpty().WithMessage("El id del paciente es requerido");
        }
    }
}
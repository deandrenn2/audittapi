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

namespace Auditt.Application.Features.Patients;

public class CreatePatient : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/patients", async (IMediator mediator, CreatePatientCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreatePatient))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<CreatePatientResponse>(StatusCodes.Status200OK);
    }
    public record CreatePatientCommand : IRequest<IResult>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string DocumentNumber { get; init; } = string.Empty;
        public DateTime BirthDate { get; init; }
        public string Eps { get; init; } = string.Empty;
    }
    public record CreatePatientResponse(int Id, string FirstName, string LastName, string DocumentType, DateTime BirthDate);
    public class CreatePatientHandler(AppDbContext context, IValidator<CreatePatientCommand> validator) : IRequestHandler<CreatePatientCommand, IResult>
    {
        public async Task<IResult> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var patient = Patient.Create(0,request.FirstName, request.LastName,  request.DocumentNumber, request.BirthDate, request.Eps);
            await context.Patients.AddAsync(patient);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreatePatientResponse(patient.Id, patient.FirstName, patient.LastName, patient.Identification, patient.BirthDate);
                return Results.Ok(Result<Patient>.Success(patient, "Paciente creado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreatePaciente", "Error al crear el paciente")));
            }
        }
    }
    public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("El nombre del paciente es requerido");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("El apellido del paciente es requerido");
            RuleFor(x => x.DocumentNumber).NotEmpty().WithMessage("El número de documento es requerido");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("La fecha de nacimiento es requerida");
            RuleFor(x => x.BirthDate)
                .Must(BeAValidDate)
                .WithMessage("La fecha de nacimiento no es válida");
        }
        private bool BeAValidDate(DateTime date)
        {
            return date <= DateTime.Now;
        }
    }
}
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
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Features.Patients;

public class GetPatients : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/patients", async (IMediator mediator) =>
        {
            return await mediator.Send(new GetPatientsQuery());
        })
        .WithName(nameof(GetPatients))
        .WithTags(nameof(Patient))
        .ProducesValidationProblem()
        .Produces<List<GetPatientsResponse>>(StatusCodes.Status200OK);
    }
    public record GetPatientsQuery() : IRequest<IResult>;
    public record GetPatientsResponse(int Id, string FirstName, string LastName, string Identification, DateTime BirthDate, string Eps);
    public class GetPatientsHandler(AppDbContext context) : IRequestHandler<GetPatientsQuery, IResult>
    {
        public async Task<IResult> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await context.Patients.ToListAsync();
            var resModel = patients.Select(patient => new GetPatientsResponse(patient.Id, patient.FirstName, patient.LastName, patient.Identification, patient.BirthDate, patient.Eps)).ToList();
            return Results.Ok(Result<List<GetPatientsResponse>>.Success(resModel, "Pacientes obtenidos correctamente"));
        }
    }
}
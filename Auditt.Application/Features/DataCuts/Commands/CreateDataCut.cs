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

public class CreateDataCut : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/datacut", async (HttpRequest req, IMediator mediator, CreateDataCutCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateDataCut))
        .WithTags(nameof(DataCut))
        .ProducesValidationProblem()
        .Produces<CreateDataCutResponse>(StatusCodes.Status201Created);
    }
    public record CreateDataCutCommand : IRequest<IResult>
    {
        public string Name { get; set; } = string.Empty;
        public DateTime InitialDate { get; set; } 
        public DateTime FinalDate { get; set; }
        public int MaxHistory { get; set; }
        public int IdInstitucion { get; set; }
    }
    public record CreateDataCutResponse(int Id, string Name, string Cycle, DateTime InitialDate, DateTime FinalDate, int MaxHistory);
    public class CreateDataCutHandler(AppDbContext context, IValidator<CreateDataCutCommand> validator) : IRequestHandler<CreateDataCutCommand, IResult>
    {
        public async Task<IResult> Handle(CreateDataCutCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var newDataCut = DataCut.Create(0, request.Name, DateTime.Now.Year.ToString(), request.InitialDate, request.FinalDate, request.MaxHistory, request.IdInstitucion);
            context.Add(newDataCut);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateDataCutResponse(newDataCut.Id, newDataCut.Name, newDataCut.Cycle, newDataCut.InitialDate, newDataCut.FinalDate, newDataCut.MaxHistory);
                return Results.Ok(Result<DataCut>.Success(newDataCut, "Corte creado correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreateInstitucion", "Error al crear el corte")));
            }
        }

    }
    public class CreateDataCutValidator : AbstractValidator<CreateDataCutCommand>
    {
        public CreateDataCutValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacío")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres");
            RuleFor(x => x.InitialDate)
                .NotEmpty().WithMessage("La fecha inicial no puede estar vacía")
                .LessThan(x => x.FinalDate).WithMessage("La fecha inicial debe ser menor que la fecha final");
            RuleFor(x => x.FinalDate)
                .NotEmpty().WithMessage("La fecha final no puede estar vacía")
                .GreaterThan(x => x.InitialDate).WithMessage("La fecha final debe ser mayor que la fecha inicial");
            RuleFor(x => x.MaxHistory)
                .NotEmpty().WithMessage("El máximo de historia no puede estar vacío")
                .GreaterThan(0).WithMessage("El máximo de historia debe ser mayor que 0");
            RuleFor(x => x.IdInstitucion)
                .NotEmpty().WithMessage("El ID de la institución no puede estar vacío")
                .GreaterThan(0).WithMessage("El ID de la institución debe ser mayor que 0");
        }
    }
}
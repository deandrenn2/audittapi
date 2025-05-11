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
using Microsoft.EntityFrameworkCore;

public class SaveAssessment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/assessments/{id}/save", async (HttpRequest req, IMediator mediator, SaveAssessmentCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(SaveAssessment))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<SaveAssessmentResponse>(StatusCodes.Status201Created);
    }
    public record SaveAssessmentCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int IdPatient { get; set; }
        public string YearOld { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Eps { get; set; } = string.Empty;
        public List<ValuationModel> Valuations { get; set; } = new List<ValuationModel>();
        public int IdUser { get; set; }

    }

    public record ValuationModel(int Id, int Order, string Text, int IdAssessment, int IdEquivalence, int? IdQuestion);
    public record SaveAssessmentResponse(int Id, int IdDataCut);
    public class SaveAssessmentHandler(AppDbContext context, IValidator<SaveAssessmentCommand> validator) : IRequestHandler<SaveAssessmentCommand, IResult>
    {
        public async Task<IResult> Handle(SaveAssessmentCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }

            var findAssessment = await context.Assessments.Include(x => x.Valuations).Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (findAssessment == null)
            {
                return Results.Ok(Result.Failure(new Error("Assessment.NotFound", "No se encontró la evaluación")));
            }

            findAssessment.Update(request.YearOld, request.Date, request.Eps, request.IdUser);
            if (findAssessment.Valuations.Count > 0)
            {
                findAssessment.Valuations.ForEach(x =>
                {
                    var value = request.Valuations.Find(y => y.Id == x.Id);
                    if (value == null)
                        return;
                    x.UpdateEqui(value.IdEquivalence);
                });
            }


            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                return Results.Ok(Result.Success("evaluación realizada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Assessment.ErrorSave", "Error al guardar la evaluación")));
            }
        }
    }
    public class SaveAssessmentValidator : AbstractValidator<SaveAssessmentCommand>
    {
        public SaveAssessmentValidator()
        {
            RuleFor(x => x.IdPatient).NotEmpty().WithMessage("El paciente es requerido");
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("El usuario es requerido");
        }
    }
}
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

namespace Auditt.Application.Features.Assessments;

public class CreateAssessment : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/assessment", async (HttpRequest req, IMediator mediator, CreateAssessmentCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateAssessment))
        .WithTags(nameof(Assessment))
        .ProducesValidationProblem()
        .Produces<CreateAssessmentResponse>(StatusCodes.Status201Created);
    }
    public record CreateAssessmentCommand : IRequest<IResult>
    {
        public int IdInstitucion { get; set; }
        public int IdDataCut { get; set; }

        public int IdFunctionary { get; set; }
        public int IdPatient { get; set; }
        public int IdGuide { get; set; }
        public string YearOld { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Eps { get; set; } = string.Empty;
        public int IdUser { get; set; }

    }
    public record CreateAssessmentResponse(int Id, int IdDataCut);
    public class CreateAssessmentHandler(AppDbContext context, IValidator<CreateAssessmentCommand> validator) : IRequestHandler<CreateAssessmentCommand, IResult>
    {
        public async Task<IResult> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
        {
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.Ok(Result<IResult>.Failure(Results.ValidationProblem(result.GetValidationProblems()), new Error("Login.ErrorValidation", "Se presentaron errores de validación")));
            }
            var newAssessment = Assessment.Create(0, request.IdInstitucion, request.IdDataCut, request.IdFunctionary, request.IdPatient, request.YearOld, request.Date, request.Eps, request.IdUser, request.IdGuide);

            var guide = await context.Guides.Include(x => x.Scale).ThenInclude(x => x.Equivalences).Where(x => x.Id == request.IdGuide).FirstOrDefaultAsync(cancellationToken);
            var valuations = new List<Valuation>();
            if (guide != null)
            {
                var questions = await context.Questions.Where(x => x.IdGuide == request.IdGuide).ToListAsync(cancellationToken);
                var equivalences = guide.Scale.Equivalences.ToList();
                int idDefaultValue = equivalences.OrderBy(x => x.Value).First().Id;

                foreach (var question in questions)
                {
                    var valuation = Valuation.Create(
                        0,
                    question.Order,
                    question.Text,
                    idDefaultValue,
                    newAssessment.Id,
                    request.IdUser,
                    question.Id
                    );
                    valuations.Add(valuation);
                }


                await context.Valuations.AddRangeAsync(valuations, cancellationToken);
            }


            context.Add(newAssessment);
            var resCount = await context.SaveChangesAsync();
            if (resCount > 0)
            {
                var resModel = new CreateAssessmentResponse(newAssessment.Id, newAssessment.IdDataCut);
                return Results.Ok(Result<CreateAssessmentResponse>.Success(resModel, "Evaluación creada correctamente"));
            }
            else
            {
                return Results.Ok(Result.Failure(new Error("Login.ErrorCreateInstitucion", "Error al crear la evaluación")));
            }
        }
    }
    public class CreateAssessmentValidator : AbstractValidator<CreateAssessmentCommand>
    {
        public CreateAssessmentValidator()
        {
            RuleFor(x => x.IdInstitucion).NotEmpty().WithMessage("La institución es requerida");
            RuleFor(x => x.IdInstitucion).GreaterThan(0).WithMessage("La institución es requerida");
            RuleFor(x => x.IdDataCut).NotEmpty().WithMessage("El corte de datos es requerido");
            RuleFor(x => x.IdFunctionary).NotEmpty().WithMessage("El funcionario es requerido");
            RuleFor(x => x.IdPatient).NotEmpty().WithMessage("El paciente es requerido");
            RuleFor(x => x.YearOld).NotEmpty().WithMessage("La edad es requerida");
            RuleFor(x => x.Date).NotEmpty().WithMessage("La fecha es requerida");
            RuleFor(x => x.Eps).NotEmpty().WithMessage("La EPS es requerida");
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("El usuario es requerido");
        }
    }
}
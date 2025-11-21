namespace Auditt.Application.Domain.Models;

/// <summary>
/// Modelo para importar evaluaciones desde Excel
/// </summary>
public record AssessmentImportModel
{
    // Información del Paciente
    public string PatientIdentification { get; init; } = string.Empty;

    // Información del Funcionario
    public string FunctionaryIdentification { get; init; } = string.Empty;

    // Información de la Evaluación
    public DateTime AssessmentDate { get; init; }
    public string YearOld { get; init; } = string.Empty;
    public string Eps { get; init; } = string.Empty;

    // Valoraciones (respuestas a las preguntas)
    public Dictionary<int, string> Valuations { get; init; } = new();

    // Información del Usuario que importa
    public int UserId { get; init; }
}

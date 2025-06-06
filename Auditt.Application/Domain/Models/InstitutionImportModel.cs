namespace Auditt.Application.Domain.Models;

public record InstitutionImportModel
{
    public string Name { get; init; } = string.Empty;
    public string Abbreviation { get; init; } = string.Empty;
    public string Nit { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Manager { get; init; } = string.Empty;
    public string AssistantManager { get; init; } = string.Empty;

}
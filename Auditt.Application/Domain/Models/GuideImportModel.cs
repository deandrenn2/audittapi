namespace Auditt.Application.Domain.Models;

public record GuideImportModel
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ScaleName { get; init; } = string.Empty;
    public List<QuestionImportModel> Questions { get; init; } = new();
}

public record QuestionImportModel
{
    public string Text { get; init; } = string.Empty;
    public int Order { get; init; }
}

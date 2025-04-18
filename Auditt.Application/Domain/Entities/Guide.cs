using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Guide : AggregateRoot
{
    private Guide(int id) : base(id) { }
    public Guide(int id, string name, string description, int idScale) : base(id)
    {
        Name = name;
        Description = description;
        IdScale = idScale;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public int IdInstitution { get; private set; }
    public int IdScale { get; private set; }
    public Scale Scale { get; private set; }
    public List<Assessment> Assessments { get; private set; } = new List<Assessment>();
    public List<Question> Questions { get; private set; } = new List<Question>();

    public static Guide Create(int id, string name, string description, int idScale)
    {
        return new Guide(id, name, description, idScale);
    }

    public void Update(string name, string description, int idScale)
    {
        Name = name;
        Description = description;
        IdScale = idScale;
    }
}
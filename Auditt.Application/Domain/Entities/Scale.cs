using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Scale : AggregateRoot
{
    private Scale(int id) : base(id) { }
    public Scale(int id, string name) : base(id)
    {
        Name = name;
    }
    public string Name { get; private set; }

    public List<Equivalence> Equivalences { get; private set; } = new List<Equivalence>();

    public static Scale Create(int id, string name)
    {
        return new Scale(id, name);
    }

    public void Update(string name)
    {
        Name = name;
    }
}

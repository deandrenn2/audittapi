using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Equivalence : AggregateRoot
{
    private Equivalence(int id) : base(id) { }
    public Equivalence(int id, int idScale, string name, decimal value) : base(id)
    {
        IdScale = idScale;
        Name = name;
        Value = value;
    }

    public int IdScale { get; private set; }
    public string Name { get; private set; }
    public decimal Value { get; private set; }

    public Scale Scale { get; private set; }

    public static Equivalence Create(int id, int idScale, string name, decimal value)
    {
        return new Equivalence(id, idScale, name, value);
    }

    public void Update(string name, decimal value)
    {
        Name = name;
    }
}
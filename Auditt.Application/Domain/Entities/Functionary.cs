using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Functionary : AggregateRoot
{
    private Functionary(int id) : base(id) { }
    public Functionary(int id, string firstName, string lastName, string identification) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Identification = identification;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Identification { get; private set; }

    public static Functionary Create(int id, string firstName, string lastName, string identification)
    {
        return new Functionary(id, firstName, lastName, identification);
    }

    public void Update(string firstName, string lastName, string identification)
    {
        FirstName = firstName;
        LastName = lastName;
        Identification = identification;
    }
}
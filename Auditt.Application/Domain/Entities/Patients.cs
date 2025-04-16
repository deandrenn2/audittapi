using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Patient : AggregateRoot
{
    private Patient(int id) : base(id) { }
    public Patient(
        int id,
        string firstName,
        string lastName,
        string identification,
        string birthDate,
        string eps) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Identification = identification;
        BirthDate = birthDate;
        Eps = eps;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Identification { get; private set; }
    public string BirthDate { get; private set; }
    public string Eps { get; private set; }

    public static Patient Create(int id, string firstName, string lastName, string identification, string birthDate, string eps)
    {
        return new Patient(id, firstName, lastName, identification, birthDate, eps);
    }

    public void Update(string firstName, string lastName, string identification, string birthDate, string eps)
    {
        FirstName = firstName;
        LastName = lastName;
        Identification = identification;
        BirthDate = birthDate;
        Eps = eps;
    }
}
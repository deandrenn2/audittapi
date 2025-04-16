using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Log : AggregateRoot
{
    private Log(int id) : base(id) { }
    public Log(int id, int idUser, string operation, string description, DateTime date, int? idInstitution) : base(id)
    {
        IdUser = idUser;
        Operation = operation;
        Description = description;
        Date = date;
        IdInstitution = idInstitution;
    }

    public int IdUser { get; private set; }
    public string Operation { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public int? IdInstitution { get; private set; }
    public User? User { get; private set; }

    public static Log Create(int id, int idUser, string operation, string description, DateTime date, int? idInstitution)
    {
        return new Log(id, idUser, operation, description, date, idInstitution);
    }

    public void Update(int idUser, string operation, string description, DateTime date, int? idInstitution)
    {
        IdUser = idUser;
        Operation = operation;
        Description = description;
        Date = date;
        IdInstitution = idInstitution;
    }

}
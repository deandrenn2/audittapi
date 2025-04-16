using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Question : AggregateRoot
{
    private Question(int id) : base(id) { }
    public Question(int id, string name, int order, int idGuide, int idUser) : base(id)
    {
        Name = name;
        Order = order;
        IdGuide = idGuide;
        IdUserUpdate = idUser;
        IdUserCreate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
    }

    public string Name { get; private set; }
    public int Order { get; private set; }
    public int IdGuide { get; private set; }
    public Guide Guide { get; private set; }
    public int IdUserUpdate { get; private set; }
    public int IdUserCreate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }

    public static Question Create(int id, string name, int order, int idGuide, int idUser)
    {
        return new Question(id, name, order, idGuide, idUser);
    }

    public void Update(string name, int idGuide, int idUser)
    {
        Name = name;
        IdGuide = idGuide;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
    }
}
using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Question : AggregateRoot
{
    private Question(int id) : base(id) { }
    public Question(int id, string text, int order, int idGuide, int? idUser = null) : base(id)
    {
        Text = text;
        Order = order;
        IdGuide = idGuide;
        IdUserUpdate = idUser;
        IdUserCreate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
    }

    public string Text { get; private set; }
    public int Order { get; private set; }
    public int IdGuide { get; private set; }
    public Guide Guide { get; private set; }
    public int? IdUserUpdate { get; private set; }
    public int? IdUserCreate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }

    public static Question Create(int id, string text, int order, int idGuide, int? idUser = null)
    {
        return new Question(id, text, order, idGuide, idUser);
    }

    public void Update(string text, int? idUser)
    {
        Text = text;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
    }

    public void UpdateOrder(int order, int idUser)
    {
        Order = order;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
    }
}
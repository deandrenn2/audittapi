using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Question : AggregateRoot
{
    private Question(int id) : base(id) { }
    public Question(int id, string text, int order, int idGuide, int? idUser = null) : base(id)
    {
        Text = text;
        Order = order;
        GuideId = idGuide;
        IdUserUpdate = idUser;
        IdUserCreate = idUser;
        UpdateDate = DateTime.Now;
        CreateDate = DateTime.Now;
    }

    public string Text { get; private set; }
    public int Order { get; private set; }
    public int GuideId { get; private set; }
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
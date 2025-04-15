using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Valuation : AggregateRoot
{
    public Valuation(int id, int order, string text, int idEquivalence, int idAssessment, int idUser) : base(id)
    {
        Order = order;
        Text = text;
        IdEquivalence = idEquivalence;
        IdAssessment = idAssessment;
        IdUserCreated = idUser;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
    }

    public int Order { get; private set; }
    public string Text { get; private set; }
    public int IdEquivalence { get; private set; }
    public Equivalence Equivalence { get; private set; }
    public int IdAssessment { get; private set; }
    public Assessment Assessment { get; private set; }
    public int IdUserCreated { get; private set; }
    public int IdUserUpdate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }

    public static Valuation Create(int id, int order, string text, int idEquivalence, int idAssessment, int idUser)
    {
        return new Valuation(id, order, text, idEquivalence, idAssessment, idUser);
    }

    public void Update(int order, string text, int idEquivalence, int idAssessment, int idUser)
    {
        Order = order;
        Text = text;
        IdEquivalence = idEquivalence;
        IdAssessment = idAssessment;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
    }
}
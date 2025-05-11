using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Valuation : AggregateRoot
{
    private Valuation(int id) : base(id) { }
    public Valuation(int id, int order, string text, int idEquivalence, int idAssessment, int idUser, int? idQuestion) : base(id)
    {
        Order = order;
        Text = text;
        EquivalenceId = idEquivalence;
        AssessmentId = idAssessment;
        IdUserCreated = idUser;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
        IdQuestion = idQuestion;
    }

    public int Order { get; private set; }
    public string Text { get; private set; }

    public int EquivalenceId { get; private set; }
    public Equivalence Equivalence { get; private set; }
    public int AssessmentId { get; private set; }
    public Assessment Assessment { get; private set; }
    public int IdUserCreated { get; private set; }
    public int IdUserUpdate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }
    public int? IdQuestion { get; private set; }

    public static Valuation Create(int id, int order, string text, int idEquivalence, int idAssessment, int idUser, int? idQuestion = null)
    {
        return new Valuation(id, order, text, idEquivalence, idAssessment, idUser, idQuestion);
    }

    public void Update(int order, string text, int idEquivalence, int idAssessment, int idUser)
    {
        Order = order;
        Text = text;
        EquivalenceId = idEquivalence;
        AssessmentId = idAssessment;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
    }

    public void UpdateEqui(int idEquivalence)
    {
        EquivalenceId = idEquivalence;
    }
}
using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Assessment : AggregateRoot
{
    private Assessment(int id) : base(id) { }
    public Assessment(int id, int idInstitucion, int idDataCut, int idFunctionary, int idPatient, string yearOld, DateTime date, string eps, int idUser, int idGuide) : base(id)
    {
        InstitutionId = idInstitucion;
        DataCutId = idDataCut;
        FunctionaryId = idFunctionary;
        PatientId = idPatient;
        YearOld = yearOld;
        Date = date;
        Eps = eps;
        IdUserCreated = idUser;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
        GuideId = idGuide;
    }

    public int InstitutionId { get; private set; }
    public Institution Institution { get; private set; }
    public int DataCutId { get; private set; }
    public DataCut DataCut { get; private set; }
    public int FunctionaryId { get; private set; }
    public Functionary Functionary { get; private set; }
    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public string YearOld { get; private set; }
    public DateTime Date { get; private set; }
    public int GuideId { get; private set; }
    public Guide Guide { get; private set; }
    public string Eps { get; private set; }
    public int IdUserCreated { get; private set; }
    public int IdUserUpdate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }
    public List<Valuation> Valuations { get; private set; } = new List<Valuation>();

    public static Assessment Create(int id, int idInstitucion, int idDataCut, int idFunctionary, int idPatient, string yearOld, DateTime date, string eps, int idUser, int idGuide)
    {
        return new Assessment(id, idInstitucion, idDataCut, idFunctionary, idPatient, yearOld, date, eps, idUser, idGuide);
    }

    public void AddValuations(List<Valuation> valuations)
    {
        Valuations = valuations;
    }
    public void AddValuation(Valuation valuation)
    {
        Valuations.Add(valuation);
    }

    public void Update(int idInstitucion, int idDataCut, int idFunctionary, DateTime date, string eps, string yearOld, int idUser, int idGuide)
    {
        InstitutionId = idInstitucion;
        DataCutId = idDataCut;
        FunctionaryId = idFunctionary;
        YearOld = yearOld;
        Date = date;
        Eps = eps;
        IdUserUpdate = idUser;
        GuideId = idGuide;
        UpdateDate = DateTime.Now;
    }
}
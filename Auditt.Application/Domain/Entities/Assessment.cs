using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Assessment : AggregateRoot
{
    public Assessment(int id, int idInstitucion, int idDataCut, int idFunctionary, int idPatient, string yearOld, DateTime date, string eps, int idUser) : base(id)
    {
        IdInstitution = idInstitucion;
        IdDataCut = idDataCut;
        IdFunctionary = idFunctionary;
        IdPatient = idPatient;
        YearOld = yearOld;
        Date = date;
        Eps = eps;
        IdUserCreated = idUser;
        IdUserUpdate = idUser;
        UpdateDate = new DateTime();
        CreateDate = new DateTime();
    }

    public int IdInstitution { get; private set; }
    public Institution Institution { get; private set; }
    public int IdDataCut { get; private set; }
    public DataCut DataCut { get; private set; }
    public int IdFunctionary { get; private set; }
    public Functionary Functionary { get; private set; }
    public int IdPatient { get; private set; }
    public Patient Patient { get; private set; }
    public string YearOld { get; private set; }
    public DateTime Date { get; private set; }
    public string Eps { get; private set; }
    public int IdUserCreated { get; private set; }
    public int IdUserUpdate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    public DateTime CreateDate { get; private set; }
    public List<Valuation> Valuations { get; private set; } = new List<Valuation>();

    public static Assessment Create(int id, int idInstitucion, int idDataCut, int idFunctionary, int idPatient, string yearOld, DateTime date, string eps, int idUser)
    {
        return new Assessment(id, idInstitucion, idDataCut, idFunctionary, idPatient, yearOld, date, eps, idUser);
    }

    public void Update(int idInstitucion, int idDataCut, int idFunctionary, DateTime date, string eps, string yearOld, int idUser)
    {
        IdInstitution = idInstitucion;
        IdDataCut = idDataCut;
        IdFunctionary = idFunctionary;
        YearOld = yearOld;
        Date = date;
        Eps = eps;
        IdUserCreated = idUser;
        UpdateDate = new DateTime();
    }
}
using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class DataCut : AggregateRoot
{
    private DataCut(int id) : base(id) { }
    // Constructor requerido por EF Core
    public DataCut(int id, string name, string cycle, DateTime initialDate, DateTime finalDate, int maxHistory, int institucionId) : base(id)
    {
        Name = name;
        Cycle = cycle;
        InitialDate = initialDate;
        FinalDate = finalDate;
        MaxHistory = maxHistory;
        InstitutionId = institucionId;
    }

    public string Name { get; private set; }
    public string Cycle { get; private set; }
    public DateTime InitialDate { get; private set; }
    public DateTime FinalDate { get; private set; }
    public int MaxHistory { get; private set; }
    public int InstitutionId { get; private set; }
    public Institution Institution { get; private set; }



    public static DataCut Create(int id, string name, string cycle, DateTime InitialDate, DateTime FinalDate, int maxHistory, int institucionId)
    {
        return new DataCut(id, name, cycle, InitialDate, FinalDate, maxHistory, institucionId);
    }

    public void Update(string name, string cycle, DateTime initialDate, DateTime finalDate, int maxHistory)
    {
        Name = name;
        Cycle = cycle;
        InitialDate = initialDate;
        FinalDate = finalDate;
        MaxHistory = maxHistory;
    }
}
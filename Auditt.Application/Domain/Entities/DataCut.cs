using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class DataCut : AggregateRoot
{
    private DataCut(int id) : base(id) { }
    // Constructor requerido por EF Core
    public DataCut(int id, string name, string cycle, DateTime initialDate, DateTime finalDate, int maxHistory, Institution institution) : base(id)
    {
        Name = name;
        Description = cycle;
        InitialDate = initialDate;
        FinalDate = finalDate;
        MaxHistory = maxHistory;
        Institution = institution;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime InitialDate { get; private set; }
    public DateTime FinalDate { get; private set; }
    public int MaxHistory { get; private set; }
    public Institution Institution { get; private set; }



    public static DataCut Create(int id, string name, string cycle, DateTime InitialDate, DateTime FinalDate, int maxHistory, Institution institution)
    {
        return new DataCut(id, name, cycle, InitialDate, FinalDate, maxHistory, institution);
    }

    public void Update(string name, string cycle, int idInstitucion, DateTime initialDate, DateTime finalDate, int maxHistory)
    {
        Name = name;
        Description = cycle;
        InitialDate = initialDate;
        FinalDate = finalDate;
        MaxHistory = maxHistory;
    }
}
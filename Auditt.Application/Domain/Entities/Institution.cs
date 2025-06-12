using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Institution : AggregateRoot
{
    public Institution() : base(0) { }
    private Institution(int id) : base(id) { }
    public Institution(int id, string name, string abbreviation, string nit, string city, string manager, string assistantManager) : base(id)
    {
        Name = name;
        Abbreviation = abbreviation;
        Nit = nit;
        City = city;
        Manager = manager;
        AssistantManager = assistantManager;
    }
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
    public string Nit { get; private set; }
    public string City { get; private set; }
    public string Manager { get; private set; }
    public string AssistantManager { get; private set; }
    public int StatusId { get; private set; } = 1;
    public List<User> Users { get; private set; } = new List<User>();
    public List<DataCut> DataCuts { get; private set; } = new List<DataCut>();

    public static Institution Create(int id, string name, string abbreviation, string nit, string city, string manager, string assistantManager)
    {
        return new Institution(id, name, abbreviation, nit, city, manager, assistantManager);
    }
    public void Update(string name, string abbreviation, string nit, string city, string manager, string assistantManager)
    {
        Name = name;
        Abbreviation = abbreviation;
        Nit = nit;
        City = city;
        Manager = manager;
        AssistantManager = assistantManager;
    }

    public void ChangeStatus(int statusId)
    {
        StatusId = statusId;
    }

}
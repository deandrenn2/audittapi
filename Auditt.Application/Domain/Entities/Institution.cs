using Auditt.Application.Domain.Primitives;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Auditt.Application.Domain.Entities;

public class Institution : AggregateRoot
{
    private Institution(int id) : base(id) { }
    public Institution(int id, string name, string abbreviation, string nit, string city) : base(id)
    {
        Name = name;
        Abbreviation = abbreviation;
        Nit = nit;
        City = city;
    }
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
    public string Nit { get; private set; }
    public string City { get; private set; }
    public List<User> Users { get; private set; } = new List<User>();
    public List<DataCut> DataCuts { get; private set; } = new List<DataCut>();

    public static Institution Create(int id, string name, string abbreviation, string nit, string city)
    {
        return new Institution(id, name, abbreviation, nit, city);
    }
    public void Update(string name, string abbreviation, string nit, string city)
    {
        Name = name;
        Abbreviation = abbreviation;
        Nit = nit;
        City = city;
    }

}
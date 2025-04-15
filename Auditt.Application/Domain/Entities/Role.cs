using Auditt.Application.Domain.Primitives;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Auditt.Application.Domain.Entities;

public class Role : AggregateRoot
{
    public Role(int id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<Users> Users { get; private set; } = new List<Users>();
    public List<Permission> Permissions { get; private set; } = new List<Permission>();

    public static Role Create(int id, string name, string description)
    {
        return new Role(id, name, description);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }


}
using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Permission : AggregateRoot
{
    private Permission(int id) : base(id) { }
    public Permission(int id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<Role> Roles { get; private set; } = new List<Role>();


    public static Permission Create(int id, string name, string description)
    {
        return new Permission(id, name, description);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Role : AggregateRoot
{
    private Role(int id) : base(id) { }
    public Role(int id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<User> Users { get; private set; } = new List<User>();
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
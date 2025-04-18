using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;

public class Permission : AggregateRoot
{
    private Permission(int id) : base(id) { }
    public Permission(int id, string name, string code, string? description = null) : base(id)
    {
        Name = name;
        Code = code;
        Description = description;
    }
    public string Name { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public List<Role> Roles { get; private set; } = new List<Role>();


    public static Permission Create(int id, string name, string code, string? description = null)
    {
        return new Permission(id, name, code, description);
    }

    public void Update(string name, string code, string? description = null)
    {
        Name = name;
        Code = code;
        Description = description;
    }
}
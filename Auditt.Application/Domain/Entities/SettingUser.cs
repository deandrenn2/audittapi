using Auditt.Application.Domain.Primitives;

namespace Auditt.Application.Domain.Entities;
public class SettingUser : AggregateRoot
{
    private SettingUser(int id) : base(id) { }
    public SettingUser(int id, int idSetting, int idUser, string value) : base(id)
    {
        IdSetting = idSetting;
        IdUser = idUser;
        Value = value;
    }

    public int IdSetting { get; private set; }
    public int IdUser { get; private set; }
    public string Value { get; private set; }
    public User User { get; private set; }
    public Setting Setting { get; private set; }

    public void Update(int idSetting, int idUser, string value)
    {
        IdSetting = idSetting;
        IdUser = idUser;
        Value = value;
    }

}

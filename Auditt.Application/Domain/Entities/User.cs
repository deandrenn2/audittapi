using Auditt.Application.Domain.Enums;
using Auditt.Application.Domain.Extensions;
using Auditt.Application.Domain.Primitives;
using Auditt.Domain.Extensions;
using Auditt.Domain.Shared;

namespace Auditt.Application.Domain.Entities;

public class User : AggregateRoot
{

    private User(int id) : base(id) { }

    public User(int id,
            string firstName,
            string lastName,
            string email,
            string passWord,
            string securePharse,
            int idRol
            ) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = passWord;
        SecurePharse = securePharse;
        RoleId = idRol;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public int RoleId { get; private set; }
    public Role Role { get; private set; }
    public string SecurePharse { get; private set; }
    public int StatusId { get; private set; } = 1;
    public int IdAvatar { get; private set; } = NumberRandom.Random(1, 20);

    public static User Create(int id,
    string firstName,
    string lastName,
    string email,
    string password,
    string securePharse,
    int idRol
    )
    {
        var passwordHash = password.EncryptPassword();
        return new User(id, firstName, lastName, email, passwordHash, securePharse, idRol);
    }


    public Result Login(string password)
    {
        var isPasswordMatch = Password == password.EncryptPassword();

        if (!isPasswordMatch)
        {
            return Result.Failure(new Error("Autentication.NotMatchPassword", "Credenciales de acceso no validas"));
        }

        var isActive = (UserStatus)StatusId == UserStatus.Active;

        if (!isActive)
        {
            return Result.Failure(new Error("Autentication.NotActive", "Usuario inactivo o bloqueado"));
        }

        //AddDomainEvent(new UserLoginDomainEvent(Guid.NewGuid(), this));

        return Result.Success("Autenticado correctamente");
    }

    public bool ValidateSecurePharse(string securePharse)
    {
        return SecurePharse.Trim().ToUpper() == securePharse.Trim().ToUpper();
    }

    public void UpdatePassword(string password)
    {
        Password = password.EncryptPassword();
    }

    public void Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void UpdateRole(int idRol)
    {
        RoleId = idRol;
    }

    public void SetAvatar(int id)
    {
        IdAvatar = id;
    }
}


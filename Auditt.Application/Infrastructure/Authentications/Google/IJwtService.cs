using Auditt.Application.Domain.Entities;

namespace Auditt.Application.Infrastructure.Authentications.Google;

public interface IJwtService
{
    string GenerateToken(User user);
    string? ValidateToken(string token); // Devuelve email o null
}

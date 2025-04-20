using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auditt.Application.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Auditt.Application.Infrastructure.Authentications.Google;

public class JwtService : IJwtService
{
    private readonly string _key = "clave-ultra-secreta-para-firma";
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidateToken(string token)
    {
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                ValidateIssuerSigningKey = true
            }, out var _);

            return principal.FindFirstValue(ClaimTypes.Email);
        }
        catch
        {
            return null;
        }
    }
}

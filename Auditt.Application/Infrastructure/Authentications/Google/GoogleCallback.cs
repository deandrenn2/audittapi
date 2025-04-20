using System.Net.Http.Json;
using System.Text.Json;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Auditt.Application.Infrastructure.Authentications.Google;

public record GoogleCallbackQuery(string Code) : IRequest<IResult>;

public class GoogleCallbackHandler(AppDbContext context, IHttpClientFactory http, IHttpContextAccessor httpContextAccessor, IJwtService _jwt) : IRequestHandler<GoogleCallbackQuery, IResult>
{

    public async Task<IResult> Handle(GoogleCallbackQuery request, CancellationToken ct)
    {
        var clientId = "TU_CLIENT_ID";
        var clientSecret = "TU_SECRET";
        var redirectUri = "https://localhost:5001/auth/google-callback";

        var client = http.CreateClient();

        var tokenRes = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = request.Code,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        }), ct);

        var tokenBody = await tokenRes.Content.ReadFromJsonAsync<JsonElement>();
        var accessToken = tokenBody.GetProperty("access_token").GetString();

        var userRes = await client.GetFromJsonAsync<JsonElement>($"https://www.googleapis.com/oauth2/v2/userinfo?access_token={accessToken}", ct);
        var email = userRes.GetProperty("email").GetString();
        var name = userRes.GetProperty("name").GetString();

        var user = await context.Users.FindAsync(email);
        if (user is null)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                user = User.Create(0, name, "APELLIDO", email, "PASSWORD", "SECUREPHRASE", 1);
                await context.SaveChangesAsync();
            }
            else
            {
                return Results.BadRequest("No se pudo obtener el nombre o el correo electrónico del usuario.");
            }
        }

        var jwt = _jwt.GenerateToken(user);

        if (httpContextAccessor.HttpContext != null)
        {
            var response = httpContextAccessor.HttpContext.Response;
            response.Cookies.Append("access_token", jwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }  

        return Results.Redirect("http://localhost:5173/dashboard");
    }
}

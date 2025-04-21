using System.Net.Http.Json;
using System.Text.Json;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Authentications;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auditt.Application.Infrastructure.Authentications.Google;

public class GoogleCallbackEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/google-callback", async ([AsParameters] GoogleCallbackQuery query, ISender sender) =>
        {
            return await sender.Send(query);
        })
        .WithName("GoogleCallback")
        .WithTags("Login")
        .Produces(StatusCodes.Status200OK);
    }

    public record GoogleCallbackQuery(string Code) : IRequest<IResult>;

    public class GoogleCallbackHandler(AppDbContext context, IHttpClientFactory http, IHttpContextAccessor httpContextAccessor, IManagerToken _jwt, IConfiguration configuration) : IRequestHandler<GoogleCallbackQuery, IResult>
    {

        public async Task<IResult> Handle(GoogleCallbackQuery request, CancellationToken ct)
        {
            // Obtén la configuración de Google desde appsettings.json
            var googleConfig = configuration.GetSection("GoogleOAuth2");
            if (googleConfig is null)
            {
                return Results.BadRequest("No se encontró la configuración de Google OAuth2.");
            }
            var webSiteConfig = configuration.GetSection("WebSite");
            if (webSiteConfig is null)
            {
                return Results.BadRequest("No se encontró la configuración del sitio web.");
            }
            var webApiConfig = configuration.GetSection("WebApi");
            if (webApiConfig is null)
            {
                return Results.BadRequest("No se encontró la configuración de la API web.");
            }

            var clientId = googleConfig["ClientId"];
            var clientSecret = googleConfig["ClientSecret"];
            var redirectUri = $"{webApiConfig["Url"]}{googleConfig["RedirectUri"]}";

            var client = http.CreateClient();

            var tokenRes = await client.PostAsync($"{googleConfig["Url"]}/token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = request.Code,
                ["client_id"] = clientId ?? "",
                ["client_secret"] = clientSecret ?? "",
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code"
            }), ct);

            var tokenBody = await tokenRes.Content.ReadFromJsonAsync<JsonElement>();
            var accessToken = tokenBody.GetProperty("access_token").GetString();

            var userRes = await client.GetFromJsonAsync<JsonElement>($"{googleConfig["Url"]}/oauth2/v2/userinfo?access_token={accessToken}", ct);
            var email = userRes.GetProperty("email").GetString();
            var name = userRes.GetProperty("name").GetString();

            var user = await context.Users.Where(x => x.Email == email).FirstOrDefaultAsync(ct);
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
                response.Cookies.Append("access_token", jwt.Token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = false,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
            }

            return Results.Redirect($"{webSiteConfig["Url"]}/dashboard");
        }
    }

}


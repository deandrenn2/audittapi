using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auditt.Application.Infrastructure.Authentications.Google;
public static class GoogleLoginEndpoint
{
    public static IEndpointRouteBuilder MapGoogleLogin(this IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/google-login", (HttpContext http) =>
        {
            var clientId = "TU_GOOGLE_CLIENT_ID";
            var redirectUri = "https://localhost:5001/auth/google-callback";

            var url = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=openid%20email%20profile";

            return Results.Redirect(url);
        });

        return app;
    }
}

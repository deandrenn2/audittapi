using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Auditt.Application.Infrastructure.Authentications.Google;
public class GoogleLoginEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("api/auth/google-login", async (HttpContext req, IMediator mediator) =>
        {
            return await mediator.Send(new GoogleLoginQuery());
        })
        .WithName("GoogleLogin")
        .WithTags("Login");
    }

    public record GoogleLoginQuery() : IRequest<IResult>;
    public class GoogleLoginHandler(IConfiguration configuration) : IRequestHandler<GoogleLoginQuery, IResult>
    {
        public Task<IResult> Handle(GoogleLoginQuery request, CancellationToken ct)
        {
            var googleConfig = configuration.GetSection("GoogleOAuth2");
            if (googleConfig is null)
            {
                return Task.FromResult<IResult>(Results.BadRequest("No se encontró la configuración de Google OAuth2."));
            }

            var clientId = googleConfig["ClientId"];
            var redirectUri = $"{googleConfig["RedirectUri"]}";
            var urlBaseAccount = googleConfig["UrlAccount"] ?? "https://accounts.google.com";

            var url = $"{urlBaseAccount}/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=openid%20email%20profile";


            Console.WriteLine($"Generated Google OAuth2 URL: {url}");

            return Task.FromResult<IResult>(Results.Redirect(url));
        }
    }

}

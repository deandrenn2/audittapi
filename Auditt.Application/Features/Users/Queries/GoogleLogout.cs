using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Auditt.Application.Features.Users.Commands;

public class GoogleLogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auth/google-logout", async (HttpContext req, IMediator mediator) =>
        {
            return await mediator.Send(new GoogleLogoutCommand());
        })
        .WithName("GoogleLogout")
        .WithTags("Login");
    }

    public record GoogleLogoutCommand() : IRequest<IResult>;

    public class GoogleLogoutHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GoogleLogoutCommand, IResult>
    {
        public Task<IResult> Handle(GoogleLogoutCommand request, CancellationToken ct)
        {
            var webSiteConfig = configuration.GetSection("WebSite");
            if (webSiteConfig is null)
            {
                return Task.FromResult(Results.BadRequest("No se encontró la configuración del sitio web."));
            }

            var loginUri = webSiteConfig["LoginUri"] ?? "";
            var urlBase = webSiteConfig["Url"] ?? "";
            var redirectUri = $"{urlBase}{loginUri}";

            if (httpContextAccessor.HttpContext != null)
            {
                var response = httpContextAccessor.HttpContext.Response;
                response.Cookies.Delete("access_token");
            }

            return Task.FromResult(Results.Redirect(redirectUri));
        }
    }
}
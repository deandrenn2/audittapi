using Auditt.Application.Infrastructure.Authentications.Google;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auditt.Application.Infrastructure.Authentications.Google;

public static class GoogleCallbackEndpoint
{
    public static IEndpointRouteBuilder MapGoogleCallback(this IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/google-callback", async (
            [AsParameters] GoogleCallbackQuery query,
            ISender sender) =>
        {
            return await sender.Send(query);
        });

        return app;
    }
}

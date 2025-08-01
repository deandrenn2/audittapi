using Microsoft.AspNetCore.Http;

namespace Auditt.Application.Infrastructure.Authorization;

public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;

    public JwtCookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Si no hay Authorization header pero hay cookie access_token
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) &&
            context.Request.Cookies.TryGetValue("access_token", out var token) &&
            !string.IsNullOrEmpty(token))
        {
            // Establecer el token en el header para que el middleware JWT lo procese
            context.Request.Headers.Authorization = $"Bearer {token}";
            Console.WriteLine($"JWT Cookie Middleware: Token moved from cookie to header");
        }

        await _next(context);
    }
}

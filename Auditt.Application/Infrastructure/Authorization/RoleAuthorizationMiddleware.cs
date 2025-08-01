using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Auditt.Application.Infrastructure.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Infrastructure.Authorization;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var endpoint = context.GetEndpoint();
        var requireRole = endpoint?.Metadata.GetMetadata<RequireRoleAttribute>();

        if (requireRole != null)
        {
            // Verificar si el usuario está autenticado
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("No autorizado");
                return;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Usuario no válido");
                return;
            }

            // Obtener el rol del usuario desde la base de datos
            var userRole = await dbContext.Users
                .Where(u => u.Id == userIdInt)
                .Include(u => u.Role)
                .Select(u => u.Role.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(userRole))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Usuario sin rol asignado");
                return;
            }

            // Verificar si el rol del usuario está en los roles permitidos
            if (!requireRole.Roles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"Acceso denegado. Se requiere rol: {string.Join(", ", requireRole.Roles)}");
                return;
            }
        }

        await _next(context);
    }
}

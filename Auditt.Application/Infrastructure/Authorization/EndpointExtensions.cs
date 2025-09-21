using Microsoft.AspNetCore.Builder;

namespace Auditt.Application.Infrastructure.Authorization;

public static class EndpointExtensions
{
    /// <summary>
    /// Requiere que el usuario tenga uno de los roles especificados
    /// </summary>
    public static TBuilder RequireRole<TBuilder>(this TBuilder builder, params string[] roles)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.WithMetadata(new RequireRoleAttribute(roles));
    }

    /// <summary>
    /// Solo permite acceso a usuarios con rol ADMIN
    /// </summary>
    public static TBuilder RequireAdmin<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireRole("ADMIN");
    }

    /// <summary>
    /// Permite acceso a ADMIN y AUDITOR INTERNO
    /// </summary>
    public static TBuilder RequireAdminOrInterno<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireRole("ADMIN", "AUDITOR INTERNO");
    }

    /// <summary>
    /// Solo permite acceso a usuarios con rol AUDITOR INTERNO
    /// </summary>
    public static TBuilder RequireInterno<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireRole("AUDITOR INTERNO");
    }
}

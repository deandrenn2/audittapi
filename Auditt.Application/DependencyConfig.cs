using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Auditt.Application.Infrastructure.Files;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Authentications;
using Auditt.Infrastructure.Authentications;
using System.Text;
using Auditt.Application.Domain.Entities;

namespace Auditt.Application;

public static class DependencyConfig
{


    public static void AddAutenticationServices(this WebApplicationBuilder builder)
    {

        // Agrega el servicio de autorización
        builder.Services.AddAuthorization();

        // Obtener JwtSettings desde configuración con múltiples métodos de lectura
        var jwtIssuer = builder.Configuration["JwtSettings:Issuer"]
                       ?? builder.Configuration["JwtSettings__Issuer"]
                       ?? Environment.GetEnvironmentVariable("JwtSettings__Issuer");

        var jwtAudience = builder.Configuration["JwtSettings:Audience"]
                         ?? builder.Configuration["JwtSettings__Audience"]
                         ?? Environment.GetEnvironmentVariable("JwtSettings__Audience");

        var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"]
                          ?? builder.Configuration["JwtSettings__SecretKey"]
                          ?? Environment.GetEnvironmentVariable("JwtSettings__SecretKey");

        // Debug: Imprimir todas las variables de entorno JWT disponibles
        Console.WriteLine("=== JWT Configuration Debug ===");
        Console.WriteLine($"ENV JwtSettings__Issuer: '{Environment.GetEnvironmentVariable("JwtSettings__Issuer")}'");
        Console.WriteLine($"ENV JwtSettings__Audience: '{Environment.GetEnvironmentVariable("JwtSettings__Audience")}'");
        Console.WriteLine($"ENV JwtSettings__SecretKey length: {Environment.GetEnvironmentVariable("JwtSettings__SecretKey")?.Length ?? 0}");
        Console.WriteLine($"Config JwtSettings:Issuer: '{builder.Configuration["JwtSettings:Issuer"]}'");
        Console.WriteLine($"Config JwtSettings:SecretKey length: {builder.Configuration["JwtSettings:SecretKey"]?.Length ?? 0}");
        Console.WriteLine($"Final - Issuer: '{jwtIssuer}', Audience: '{jwtAudience}', SecretKey length: {jwtSecretKey?.Length ?? 0}");
        Console.WriteLine("=== End JWT Debug ===");

        // Validar que las configuraciones JWT estén presentes
        if (string.IsNullOrEmpty(jwtSecretKey))
        {
            throw new InvalidOperationException("JWT SecretKey is not configured. Please set JwtSettings:SecretKey in appsettings.json or JwtSettings__SecretKey as environment variable.");
        }

        if (string.IsNullOrEmpty(jwtIssuer))
        {
            throw new InvalidOperationException("JWT Issuer is not configured. Please set JwtSettings:Issuer in appsettings.json or JwtSettings__Issuer as environment variable.");
        }

        if (string.IsNullOrEmpty(jwtAudience))
        {
            throw new InvalidOperationException("JWT Audience is not configured. Please set JwtSettings:Audience in appsettings.json or JwtSettings__Audience as environment variable.");
        }

        // Configuración del servicio de autenticación JWT
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
            };
        });
    }

    public static void AddInfraestructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IManagerToken, ManagerToken>();
        builder.Services.AddScoped<IFileManager, FileManager>();
        builder.Services.AddScoped<IExcelImporter<Institution>, InstitutionExcelImporter>();
        builder.Services.AddScoped<IExcelImporter<Patient>, PatientExcelImporter>();
        builder.Services.AddScoped<IExcelImporter<Functionary>, FunctionaryExcelImporter>();
        builder.Services.AddScoped<GuideExcelImporter>();
    }

    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddAutoMapper(typeof(Application));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Application).Assembly);
            //config.AddOpenBehavior(typeof(TransactionBehaviour<,>));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(Application));


        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var connectionString = string.Empty;

        if (enviroment == "Development")
        {
            connectionString = config.GetConnectionString("SqliteConn");
        }
        else
        {
            var basePath = AppContext.BaseDirectory;
            var relativePath = config.GetConnectionString("SqliteConn");
            if (relativePath == null)
            {
                return services;
            }

            var fullPath = Path.Combine(basePath, relativePath.Replace("Data Source=", string.Empty));
            connectionString = $"Data Source={fullPath}";
        }

        Console.WriteLine(connectionString);
        Console.WriteLine("Deimer conexion");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }


    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Configuración de CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost:5000", "http://localhost:7045", "http://audittweb.host.imagyapp.net", "https://audittweb.host.imagyapp.net", "https://audittapi.host.imagyapp.net")  // Permite cualquier origen
                .AllowCredentials()
                .AllowAnyMethod()  // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
                .AllowAnyHeader(); // Permite cualquier cabecera

            });
        });

        return services;
    }


}
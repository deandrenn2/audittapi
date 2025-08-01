﻿using Carter;
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

        // Optener JwtSettings desde appsettings.json
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");

        // Congiguración del servicio de autenticación JWT
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
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? ""))
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
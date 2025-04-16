using Auditt.Application;
using Carter;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Reports;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title = "OPTIC Api";
    c.Version = "v1";
});

// Configurar DbContext con MediatR
builder.Services.AddApplicationCore();

// Configurar DbContext con SQLite
builder.Services.AddPersistence(builder.Configuration);

// Autorizacion y autenticacion
builder.AddAutenticationServices();
builder.Services.ConfigureServices();

// registro de servicios
builder.AddInfraestructure();
builder.Services.AddInfraestructureReports();

var app = builder.Build();

// 🔄 Aplica las migraciones automáticamente si no estás en desarrollo
if (!app.Environment.IsDevelopment())
{
    await app.MigrateDatabaseAsync(); // <- Esta línea es clave
}
app.UseHttpsRedirection();
app.UseOpenApi();
app.UseSwaggerUi(settings => { settings.Path = "/docs"; });
app.UseReDoc(settings =>
{
    settings.Path = "/redoc";
    settings.DocumentPath = "/swagger/v1/swagger.json";
});
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.MapCarter();
app.Run();

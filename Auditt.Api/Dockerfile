# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el proyecto principal
COPY ["Auditt.Api/Auditt.Api.csproj", "Auditt.Api/"]
RUN dotnet restore "Auditt.Api/Auditt.Api.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/Auditt.Api"
RUN dotnet publish -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Crear directorio App_Data con permisos
RUN mkdir -p /app/App_Data && \
    chown -R app:app /app/App_Data && \
    chmod -R 755 /app/App_Data

# Configurar variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

# Exponer el puerto
EXPOSE 8080
# Usar usuario no root para seguridad
USER app

ENTRYPOINT ["dotnet", "Auditt.Api.dll"]
# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solamente los archivos de proyecto para restaurar dependencias
COPY ["Auditt.Api/Auditt.Api.csproj", "Auditt.Api/"]
COPY ["Auditt.Application/Auditt.Application.csproj", "Auditt.Application/"]
COPY ["Auditt.Reports/Auditt.Reports.csproj", "Auditt.Reports/"]
RUN dotnet restore "Auditt.Api/Auditt.Api.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/Auditt.Api"
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Crear la carpeta App_Data y asegurar los permisos
RUN mkdir -p /app/App_Data && \
    chmod 777 /app/App_Data

# Configurar variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    SQLITE_CONN="Data Source=/app/App_Data/audittDb.sqlite"

# Exponer el puerto 80
EXPOSE 80

# Entrypoint
ENTRYPOINT ["dotnet", "Auditt.Api.dll"]
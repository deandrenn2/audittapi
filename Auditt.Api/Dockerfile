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

# Configurar variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "Auditt.Api.dll"]
# Variables de Entorno para Dokploy - Auditt API

Configura estas variables en el panel de Dokploy:

## 🔧 Configuración de Base de Datos

```
ConnectionStrings__SqliteConn=Data Source=/app/App_Data/opticDb_prod.sqlite
```

## 🔐 Configuración JWT (REQUERIDA)

```
JwtSettings__Issuer=TU_ISSUER_AQUI
JwtSettings__Audience=TU_AUDIENCE_AQUI
JwtSettings__SecretKey=TU_SECRET_KEY_SEGURO_AQUI
JwtSettings__ExpirationMinutes=120
```

## 🌐 Configuración OAuth Google

```
GOOGLE_OAUTH_CLIENT_ID=tu_client_id_real
GOOGLE_OAUTH_CLIENT_SECRET=tu_client_secret_real
```

## ⚙️ Configuración del Sistema

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
DOTNET_RUNNING_IN_CONTAINER=true
```

## 📋 Logging (Opcional)

```
LOGGING__LOGLEVEL__DEFAULT=Information
LOGGING__LOGLEVEL__MICROSOFT_ASPNETCORE=Warning
```

---

## 🚨 IMPORTANTE:

Sin las variables JWT, la aplicación fallará al arrancar. Asegúrate de configurar todas las variables `JwtSettings__*` en Dokploy.

## 📂 Configuración de Volúmenes:

- **Mount Type**: File Mount
- **Container Path**: `/app/App_Data`
- **Host Path**: (Dokploy lo maneja automáticamente)

## 🔄 Después de configurar:

1. Guarda las variables de entorno
2. Haz clic en "Deploy" o "Redeploy"
3. Verifica que la aplicación inicie correctamente

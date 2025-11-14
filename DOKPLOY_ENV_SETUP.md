# Variables de Entorno para Dokploy - Auditt API

Configura estas variables en el panel de Dokploy:

## 🔧 Configuración de Base de Datos

```
ConnectionStrings__SqliteConn=Data Source=/app/App_Data/opticDb_prod.sqlite
```

## 🔐 Configuración JWT (REQUERIDA)

⚠️ **IMPORTANTE**: Usa exactamente estos nombres con doble underscore `__`

```
JwtSettings__Issuer=AUDITT-GPC
JwtSettings__Audience=AUDITT-GPC-API
JwtSettings__SecretKey=TU_SECRET_KEY_SEGURO_AQUI
JwtSettings__ExpirationMinutes=120
```

### 🔍 Para Verificar la Configuración:

1. Después del deploy, revisa los logs de la aplicación
2. Busca la sección "=== JWT Configuration Debug ==="
3. Verifica que las variables aparezcan correctamente
4. Si alguna variable está vacía, ajusta la configuración en Dokploy

## 🌐 Configuración OAuth Google

```
GoogleOAuth__ClientId=TU_CLIENT_ID_AQUI
GoogleOAuth__ClientSecret=TU_CLIENT_SECRET_AQUI
GoogleOAuth__RedirectUri=/api/auth/google-callback
GoogleOAuth__Url=https://oauth2.googleapis.com
GoogleOAuth__UrlAccount=https://accounts.google.com
GoogleOAuth__UrlApi=https://www.googleapis.com
```

## 🌍 Configuración de Website y API

```
WebSite__Url=TU_WEBSITE_URL_AQUI
WebSite__LoginUri=/login
WebApi__Url=TU_API_URL_AQUI
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

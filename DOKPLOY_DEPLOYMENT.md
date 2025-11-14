# Despliegue de Auditt API con Dokploy

## Archivos de Configuración

### 1. dokploy.json

Configuración principal para Dokploy con:

- Puerto 80 (igual que CapRover)
- Variables de entorno de ASP.NET Core
- Volumen persistente para App_Data
- Health checks configurados
- Recursos limitados (1GB RAM, 0.5 CPU cores)

### 2. docker-compose.yml

Archivo alternativo para despliegue con Docker Compose:

- Misma configuración de puertos y variables
- Health checks con curl
- Red externa de Dokploy
- Volúmenes persistentes

### 3. .env.production

Variables de entorno específicas para producción.

## Pasos para Desplegar en Dokploy

1. **Configurar el proyecto en Dokploy:**

   - Crear nuevo proyecto
   - Conectar repositorio Git
   - Seleccionar rama (master)

2. **Configuración de despliegue:**

   - Tipo: Docker
   - Dockerfile: `./Auditt.Api/Dockerfile`
   - Build context: `.` (raíz del proyecto)

3. **Variables de entorno:**

   - Configurar en el panel de Dokploy o usar `.env.production`
   - Especialmente importante: `GOOGLE_OAUTH_CLIENT_ID` y `GOOGLE_OAUTH_CLIENT_SECRET`

4. **Volúmenes:**

   - Configurar volumen persistente para `/app/App_Data`
   - Importante para mantener datos entre deployments

5. **Dominio:**
   - Configurar dominio personalizado
   - Habilitar HTTPS automático

## Diferencias con CapRover

- Dokploy usa configuración JSON en lugar de `captain-definition`
- Mejor integración con Docker Compose
- Health checks más avanzados
- Gestión de recursos más granular

## Verificación

Después del despliegue, verificar:

- API responde en el puerto 80
- Health check endpoint funciona
- Volúmenes persistentes montados correctamente
- Variables de entorno cargadas

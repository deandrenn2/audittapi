# Sistema de Importación de Excel Mejorado

## Descripción

El sistema de importación de Excel ha sido mejorado para manejar validaciones de índices únicos y proporcionar información detallada sobre el proceso de importación.

## Características Principales

### 1. Validación de Índices Únicos

- **Detección previa**: Antes de intentar guardar en la base de datos, el sistema verifica si ya existe una entidad con el mismo valor único.
- **Continuación del proceso**: Si se encuentra un duplicado, se omite esa fila y se continúa con las siguientes.
- **Manejo de errores**: Se capturan las excepciones de restricciones únicas a nivel de base de datos.

### 2. Información Detallada de Importación

- **Estadísticas completas**: Número de registros exitosos, duplicados y errores
- **Detalles de errores**: Lista específica de filas con problemas y descripciones
- **Resumen ejecutivo**: Mensaje consolidado del resultado de la importación

## Implementación Técnica

### Clase Base Abstracta: `ExcelImporter<T>`

#### Métodos Requeridos para Implementar

```csharp
protected abstract string[] ColumnHeaders { get; }
protected abstract T? CreateEntityFromRow(IXLRow row);
protected abstract void FormatTemplate(IXLWorksheet worksheet);
protected abstract bool EntityExists(T entity, AppDbContext dbContext);
```

#### Nuevos Métodos Disponibles

```csharp
// Método básico (mantiene compatibilidad)
Task<Result> ImportFromExcelAsync(IFormFile file, AppDbContext dbContext)

// Método detallado (nueva funcionalidad)
Task<ImportResult<T>> ImportFromExcelDetailedAsync(IFormFile file, AppDbContext dbContext)
```

## Ejemplos de Implementación

### 1. PatientExcelImporter

```csharp
protected override bool EntityExists(Patient entity, AppDbContext dbContext)
{
    // Verifica si ya existe un paciente con la misma identificación
    return dbContext.Patients.Any(p => p.Identification == entity.Identification);
}
```

### 2. InstitutionExcelImporter

```csharp
protected override bool EntityExists(Institution entity, AppDbContext dbContext)
{
    // Verifica si ya existe una institución con el mismo NIT
    return dbContext.Institutions.Any(i => i.Nit == entity.Nit);
}
```

### 3. FunctionaryExcelImporter

```csharp
protected override bool EntityExists(Functionary entity, AppDbContext dbContext)
{
    // Verifica si ya existe un funcionario con la misma identificación
    return dbContext.Functionaries.Any(f => f.Identification == entity.Identification);
}
```

## Índices Únicos Configurados

| Entidad     | Campo Único    | Configuración                                        |
| ----------- | -------------- | ---------------------------------------------------- |
| Patient     | Identification | `builder.HasIndex(p => p.Identification).IsUnique()` |
| Institution | Nit            | `builder.HasIndex(i => i.Nit).IsUnique()`            |
| Functionary | Identification | `builder.HasIndex(f => f.Identification).IsUnique()` |
| User        | Email          | `builder.HasIndex(x => x.Email).IsUnique()`          |

## Uso en Controllers

### Endpoint Básico (Mantiene compatibilidad)

```csharp
app.MapPost("api/patients/import", async (HttpRequest req, IMediator mediator) =>
{
    var form = await req.ReadFormAsync();
    var file = form.Files["file"];
    return await mediator.Send(new ImportPatientCommand(file));
})
```

### Endpoint Detallado (Nueva funcionalidad)

```csharp
app.MapPost("api/patients/import-detailed", async (HttpRequest req, IMediator mediator) =>
{
    var form = await req.ReadFormAsync();
    var file = form.Files["file"];
    return await mediator.Send(new ImportPatientDetailedCommand(file));
})
```

## Estructura de Respuesta Detallada

```csharp
public record ImportPatientDetailedResponse(
    int SuccessfulCount,        // Número de registros importados exitosamente
    int DuplicatesCount,        // Número de duplicados omitidos
    int ErrorsCount,           // Número de errores encontrados
    int TotalProcessed,        // Total de filas procesadas
    string Summary,            // Resumen ejecutivo
    List<string> DuplicateErrors,  // Detalles de duplicados
    List<string> ValidationErrors  // Detalles de errores de validación
);
```

## Beneficios del Nuevo Sistema

1. **Robustez**: Manejo elegante de violaciones de índices únicos
2. **Transparencia**: Información completa sobre qué ocurrió durante la importación
3. **Continuidad**: El proceso no se detiene por duplicados, importa lo que puede
4. **Retrocompatibilidad**: Los endpoints existentes siguen funcionando igual
5. **Escalabilidad**: Fácil agregar validaciones personalizadas por entidad

## Casos de Uso Típicos

### Escenario 1: Importación con algunos duplicados

- **Entrada**: 100 registros, 10 duplicados, 5 con errores de formato
- **Resultado**: 85 importados exitosamente, 10 duplicados omitidos, 5 errores reportados
- **Beneficio**: Se aprovecha la mayor parte de la data válida

### Escenario 2: Actualización de datos existentes

- **Problema anterior**: Error completo si algún registro ya existía
- **Solución actual**: Omite duplicados e importa solo los nuevos
- **Beneficio**: Permite actualizaciones incrementales

### Escenario 3: Validación de integridad

- **Funcionalidad**: Verificación previa antes del guardado en BD
- **Beneficio**: Evita violaciones de restricciones y errores de transacción

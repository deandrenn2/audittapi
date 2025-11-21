# Importación de Evaluaciones (Assessments) con Contexto

## Descripción General

Sistema de importación de evaluaciones desde Excel con contexto específico de institución, corte de datos y guía. Las plantillas se generan dinámicamente mostrando las preguntas de la guía seleccionada con sus opciones de escala correspondientes.

## Arquitectura

### Modelo de Datos

**AssessmentImportModel**

```csharp
public record AssessmentImportModel
{
    public required string PatientIdentification { get; init; }
    public required string FunctionaryIdentification { get; init; }
    public required DateTime AssessmentDate { get; init; }
    public required string YearOld { get; init; }
    public required string Eps { get; init; }
    public Dictionary<int, string>? Valuations { get; init; }
    public int UserId { get; init; }
}
```

- **Valuations**: Diccionario donde la clave es el ID de la pregunta y el valor es el nombre de la equivalencia de escala seleccionada.

### Endpoints

#### 1. Generar Plantilla de Importación

**POST** `/api/assessments/template-import`

**Request Body:**

```json
{
	"institutionId": 1,
	"dataCutId": 5,
	"guideId": 3
}
```

**Response:**

- Archivo Excel (.xlsx) con plantilla pre-configurada para la institución, corte y guía especificados

**Características de la Plantilla:**

1. **Información de Contexto (Oculta)**

   - Filas 1-3: IDs de institución, corte de datos y guía (ocultas pero presentes para validación)

2. **Encabezado Visible**

   - Fila 5: Título de la plantilla
   - Filas 7-10: Información de contexto visible (Institución, Corte de Datos, Guía, Escala)

3. **Instrucciones**

   - Filas 12-16: Instrucciones detalladas para completar la plantilla

4. **Columnas de Datos (Fila 19)**

   - Identificación Paciente
   - Identificación Funcionario
   - Fecha Evaluación (formato DD/MM/YYYY)
   - Edad
   - EPS
   - [Columnas dinámicas para cada pregunta de la guía]

5. **Hoja de Referencia: "Opciones de Escala"**

   - Lista todas las equivalencias de la escala asociada a la guía
   - Nombre, Valor y Descripción de cada opción
   - Se usa para validación de listas desplegables

6. **Validaciones Automáticas**
   - Listas desplegables en las columnas de preguntas
   - Las opciones se extraen de la hoja "Opciones de Escala"

#### 2. Importar Evaluaciones

**POST** `/api/assessments/import`

**Form Data:**

- `file`: Archivo Excel (.xlsx o .xls)
- `institutionId`: ID de la institución (debe coincidir con el del archivo)
- `dataCutId`: ID del corte de datos (debe coincidir con el del archivo)
- `guideId`: ID de la guía (debe coincidir con el del archivo)

**Response:**

```json
{
	"isSuccess": true,
	"data": {
		"successCount": 10,
		"duplicateCount": 2,
		"errorCount": 1,
		"totalProcessed": 13,
		"summary": "Importación terminada. Correctamente importadas: 10, Duplicados omitidos: 2, Errores: 1",
		"duplicates": [
			"Fila 25: Ya existe una evaluación para el paciente con identificación '1234567890' en la fecha 15/01/2024"
		],
		"errors": [
			"Fila 30: La fecha de evaluación 01/01/2023 está fuera del rango del corte de datos (01/06/2023 - 31/12/2023)"
		]
	},
	"message": "Importación terminada. Correctamente importadas: 10, Duplicados omitidos: 2, Errores: 1"
}
```

## Proceso de Importación

### 1. Validación de Contexto

El sistema valida que los IDs de contexto en el archivo Excel coincidan con los parámetros proporcionados:

```csharp
var contextInstitutionId = Convert.ToInt32(worksheet.Cell(1, 2).Value);
var contextDataCutId = Convert.ToInt32(worksheet.Cell(2, 2).Value);
var contextGuideId = Convert.ToInt32(worksheet.Cell(3, 2).Value);
```

### 2. Lectura de Datos

Para cada fila (comenzando desde la fila 20):

1. **Datos Base del Paciente y Funcionario**

   - Columna 1: Identificación del Paciente
   - Columna 2: Identificación del Funcionario
   - Columna 3: Fecha de Evaluación
   - Columna 4: Edad
   - Columna 5: EPS

2. **Respuestas a Preguntas (Valoraciones)**
   - Columnas 6+: Una columna por cada pregunta de la guía
   - Se lee el nombre de la equivalencia seleccionada
   - Se almacena en el diccionario `Valuations` con el ID de la pregunta como clave

### 3. Creación de Entidades

#### Paciente (Patient)

- Si no existe, se crea con la identificación proporcionada
- Campos de nombre se dejan vacíos ("")
- Fecha de nacimiento se establece como `DateTime.Now` (no se usa para evaluaciones)
- EPS se actualiza con el valor del archivo

#### Funcionario (Functionary)

- Si no existe, se crea con la identificación proporcionada
- Campos de nombre se dejan vacíos ("")

#### Evaluación (Assessment)

- Se crea con los IDs de contexto (institución, corte de datos, guía)
- Se asocia con el paciente y funcionario
- Se valida que no exista duplicado (mismo paciente, misma guía, misma fecha)
- Se valida que la fecha esté dentro del rango del corte de datos

#### Valoraciones (Valuations)

- Se crean una por cada pregunta respondida
- Se busca la equivalencia de escala por nombre
- Se asocia con la evaluación y la pregunta
- Se asigna orden secuencial

### 4. Validaciones

#### Validación de Contexto

- IDs de contexto deben coincidir con los parámetros
- Institución, corte de datos y guía deben existir en la base de datos

#### Validación de Fechas

- Fecha de evaluación debe estar dentro del rango del corte de datos:
  ```csharp
  if (model.AssessmentDate < dataCut.InitialDate || model.AssessmentDate > dataCut.FinalDate)
  ```

#### Validación de Duplicados

- Se verifica que no exista evaluación con:
  - Mismo paciente
  - Misma guía
  - Misma fecha (solo día, sin hora)

#### Validación de Equivalencias

- Cada respuesta debe corresponder a una equivalencia válida de la escala
- La búsqueda es case-insensitive

### 5. Transacciones

Todas las evaluaciones se guardan en una única transacción:

```csharp
using var transaction = await dbContext.Database.BeginTransactionAsync();
await dbContext.SaveChangesAsync();
await transaction.CommitAsync();
```

Si hay error al guardar, se hace rollback y se reportan los errores.

## Código Clave

### AssessmentExcelImporter

**Método Principal de Generación de Plantilla:**

```csharp
public async Task<IXLWorkbook> CreateTemplateWithContextAsync(
    AppDbContext dbContext,
    int institutionId,
    int dataCutId,
    int guideId)
```

**Método Principal de Importación:**

```csharp
public async Task<ImportResult<Assessment>> ImportAssessmentsWithContextAsync(
    IFormFile file,
    AppDbContext dbContext,
    int userId,
    int institutionId,
    int dataCutId,
    int guideId)
```

**Método de Lectura de Fila:**

```csharp
private AssessmentImportModel? CreateEntityFromRowWithContext(IXLRow row, Guide guide)
```

- Lee columnas base (1-5)
- Lee respuestas dinámicas (columnas 6+) según las preguntas de la guía
- Retorna modelo con diccionario de valoraciones

**Método de Creación de Valoraciones:**

```csharp
private async Task CreateValuationsAsync(
    Assessment assessment,
    Dictionary<int, string> valuations,
    Guide guide,
    AppDbContext dbContext)
```

- Itera sobre el diccionario de valoraciones
- Busca la pregunta y la equivalencia correspondiente
- Crea entidad `Valuation` con orden secuencial

## Ejemplos de Uso

### 1. Generar Plantilla

```http
POST /api/assessments/template-import
Content-Type: application/json
Authorization: Bearer {token}

{
  "institutionId": 1,
  "dataCutId": 5,
  "guideId": 3
}
```

**Resultado:**

- Archivo `plantilla_importacion_evaluaciones.xlsx`
- Contexto pre-cargado: Hospital Central, Corte 2024-Q1, Guía de Caídas
- Columnas de preguntas: "¿Presenta mareos?", "¿Ha sufrido caídas previas?", etc.
- Opciones de escala: "Nunca", "Raramente", "Frecuentemente", "Siempre"

### 2. Completar Plantilla

**Fila de Ejemplo:**

| Identificación Paciente | Identificación Funcionario | Fecha Evaluación | Edad | EPS       | ¿Presenta mareos? | ¿Ha sufrido caídas previas? |
| ----------------------- | -------------------------- | ---------------- | ---- | --------- | ----------------- | --------------------------- |
| 1234567890              | 9876543210                 | 15/03/2024       | 68   | EPS Salud | Frecuentemente    | Raramente                   |

### 3. Importar Datos

```http
POST /api/assessments/import
Content-Type: multipart/form-data
Authorization: Bearer {token}

file: plantilla_importacion_evaluaciones.xlsx
institutionId: 1
dataCutId: 5
guideId: 3
```

**Resultado:**

- 10 evaluaciones creadas exitosamente
- 2 pacientes nuevos registrados
- 1 funcionario nuevo registrado
- 30 valoraciones creadas (3 preguntas × 10 evaluaciones)

## Manejo de Errores

### Errores Comunes

1. **"Los IDs de contexto en el archivo no coinciden con los parámetros proporcionados"**

   - Causa: Se está intentando importar un archivo generado para diferente institución/corte/guía
   - Solución: Generar nueva plantilla con los parámetros correctos

2. **"La fecha de evaluación está fuera del rango del corte de datos"**

   - Causa: La fecha en el Excel no está dentro del rango del corte de datos
   - Solución: Verificar que las fechas estén dentro del período del corte

3. **"Ya existe una evaluación para el paciente en la fecha"**

   - Causa: Duplicado detectado (mismo paciente, guía y fecha)
   - Solución: Verificar si es un duplicado intencional o error de datos

4. **"El archivo debe ser un documento Excel (.xlsx o .xls)"**
   - Causa: Formato de archivo incorrecto
   - Solución: Asegurar que el archivo sea Excel válido

## Notas Técnicas

### Compatibilidad con Versión Anterior

El método `ImportAssessmentsAsync` (sin contexto) está marcado como deprecado:

```csharp
public async Task<ImportResult<Assessment>> ImportAssessmentsAsync(
    IFormFile file,
    AppDbContext dbContext,
    int userId)
{
    errorRows.Add("Este método está deprecado. Use ImportAssessmentsWithContextAsync en su lugar");
    return new ImportResult<Assessment>(...);
}
```

### Performance

- **Queries Optimizados**: Se usan `Include()` para cargar preguntas y equivalencias en una sola consulta
- **Transacciones**: Todas las entidades se guardan en una única transacción para garantizar consistencia
- **Validación Lazy**: Las entidades relacionadas (Patient, Functionary) solo se crean/actualizan cuando es necesario

### Seguridad

- **Autenticación Requerida**: Ambos endpoints requieren token JWT válido
- **Validación de Usuario**: El UserId se extrae del token, no del request body
- **Validación de Contexto**: Los IDs en el archivo deben coincidir con los parámetros

## Limitaciones Conocidas

1. **Nombres de Pacientes/Funcionarios**: No se capturan en la importación, se dejan vacíos
2. **Fecha de Nacimiento**: No se usa para pacientes importados
3. **Edición de Evaluaciones**: No soportada, solo creación de nuevas
4. **Límite de Preguntas**: Excel tiene límite de ~16,000 columnas (suficiente para casos prácticos)

## Próximas Mejoras

- [ ] Soporte para actualización de evaluaciones existentes
- [ ] Validación de tipos de datos en Excel antes de procesar
- [ ] Reporte detallado en Excel de errores de validación
- [ ] Importación en lotes para archivos muy grandes
- [ ] Soporte para fotos/adjuntos en evaluaciones

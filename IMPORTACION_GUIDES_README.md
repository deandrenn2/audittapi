# Importación de Guías con Preguntas

## Descripción

Esta funcionalidad permite importar **una sola Guía** de evaluación junto con todas sus Preguntas asociadas desde un archivo Excel en una sola operación.

## Características Principales

### 1. Importación de Una Sola Guía

- **Una guía por archivo**: Cada archivo Excel importa exactamente una guía con todas sus preguntas
- **Información de guía en header**: Los datos de la guía se especifican en las primeras filas del Excel
- **Preguntas en filas**: Las preguntas se listan en filas consecutivas con su orden correspondiente
- **Validación de escalas**: Verifica que la escala especificada exista en el sistema

### 2. Validaciones

- **Guía única**: Detecta si la guía ya existe en el sistema
- **Escala existente**: Valida que la escala especificada exista
- **Preguntas válidas**: Valida que las preguntas tengan texto y orden numérico
- **Datos requeridos**: Valida que los campos obligatorios estén presentes

## Endpoint Disponible

### 1. Generar Template

```
POST /api/guides/template-import
```

Genera un archivo Excel con la estructura correcta para la importación.

### 2. Importar Guía

```
POST /api/guides/import
Content-Type: multipart/form-data
Body: file (Excel file)
```

Importa una guía con todas sus preguntas.

## Estructura del Excel

### Sección de Información de la Guía (Filas 1-4)

| Celda | Contenido           | Descripción                |
| ----- | ------------------- | -------------------------- |
| A2    | Name:               | Etiqueta fija              |
| B2    | [Nombre de la Guía] | Nombre único de la guía    |
| A3    | Description:        | Etiqueta fija              |
| B3    | [Descripción]       | Descripción de la guía     |
| A4    | ScaleName:          | Etiqueta fija              |
| B4    | [Nombre de Escala]  | Nombre de escala existente |

### Sección de Preguntas (Fila 7 en adelante)

| Columna A              | Columna B     |
| ---------------------- | ------------- |
| QuestionText           | QuestionOrder |
| Texto de la pregunta 1 | 1             |
| Texto de la pregunta 2 | 2             |
| Texto de la pregunta 3 | 3             |

### Ejemplo completo:

| A                                   | B                                |
| ----------------------------------- | -------------------------------- |
| **Guide Information**               |                                  |
| Name:                               | Guía de Evaluación Médica        |
| Description:                        | Evaluación completa del paciente |
| ScaleName:                          | Escala Principal                 |
|                                     |                                  |
| **Questions**                       |                                  |
| **QuestionText**                    | **QuestionOrder**                |
| ¿El paciente presenta síntomas?     | 1                                |
| ¿Se realizó el examen físico?       | 2                                |
| ¿Se registraron los signos vitales? | 3                                |

## Respuesta de la Importación

```json
{
	"success": true,
	"message": "Guía 'Evaluación Médica' importada exitosamente con 3 preguntas",
	"guideName": "Evaluación Médica",
	"questionsCount": 3,
	"errors": []
}
```

### En caso de error:

```json
{
	"success": false,
	"message": "Importación fallida: La guía ya existe",
	"guideName": null,
	"questionsCount": 0,
	"errors": ["Guide 'Evaluación Médica' already exists"]
}
```

## Casos de Uso

### Escenario 1: Nueva Guía Exitosa

- **Entrada**: Guía nueva con 5 preguntas válidas
- **Resultado**: 1 guía creada con 5 preguntas asociadas
- **Respuesta**: Success = true, QuestionsCount = 5

### Escenario 2: Escala No Existe

- **Problema**: Se especifica una escala que no existe en el sistema
- **Resultado**: Error reportado, importación fallida
- **Recomendación**: Crear la escala primero o usar una existente

### Escenario 3: Guía Duplicada

- **Problema**: Se intenta importar una guía que ya existe
- **Resultado**: Error reportado, importación fallida
- **Recomendación**: Cambiar el nombre o actualizar la guía existente

### Escenario 4: Sin Preguntas

- **Problema**: No se proporcionan preguntas válidas
- **Resultado**: Error reportado, importación fallida
- **Recomendación**: Agregar al menos una pregunta válida

## Prerrequisitos

1. **Escalas existentes**: La escala referenciada debe existir en el sistema
2. **Nombre único**: El nombre de la guía debe ser único
3. **Formato correcto**: El archivo debe seguir exactamente la estructura especificada
4. **Al menos una pregunta**: Debe haber al menos una pregunta válida

## Consideraciones Técnicas

- **Transacción atómica**: Si falla cualquier operación, se deshace toda la importación
- **Validación previa**: Se valida la existencia de la escala antes de crear la guía
- **Estructura fija**: La información de la guía debe estar en las celdas específicas
- **Orden automático**: Las preguntas se ordenan según el valor de QuestionOrder

## Limitaciones

- Solo se puede importar una guía por archivo
- Los nombres de las guías deben ser únicos en el sistema
- Las escalas deben existir previamente
- El orden de las preguntas debe ser numérico
- No se pueden actualizar guías existentes, solo crear nuevas
- La estructura del Excel es fija y no flexible

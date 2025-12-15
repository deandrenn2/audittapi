using Auditt.Application.Domain.Entities;
using Auditt.Application.Domain.Models;
using Auditt.Application.Infrastructure.Sqlite;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Auditt.Application.Infrastructure.Files;

/// <summary>
/// Importador de evaluaciones (Assessments) desde archivos Excel
/// </summary>
public class AssessmentExcelImporter : ExcelImporter<AssessmentImportModel>
{
    protected override string[] ColumnHeaders => new[]
    {
        "Identificación Paciente",
        "Identificación Funcionario",
        "Institución",
        "Corte de Datos",
        "Guía",
        "Fecha Evaluación",
        "Edad",
        "EPS"
    };

    protected override AssessmentImportModel? CreateEntityFromRow(IXLRow row)
    {
        // Este método está deprecado, use CreateEntityFromRowWithContext
        return null;
    }

    protected override bool EntityExists(AssessmentImportModel entity, AppDbContext dbContext)
    {
        // No validamos duplicados aquí, se hace en el método personalizado
        return false;
    }

    protected override void FormatTemplate(IXLWorksheet worksheet)
    {
        // Información de instrucciones en las primeras filas
        worksheet.Cell(1, 1).Value = "INSTRUCCIONES DE IMPORTACIÓN DE EVALUACIONES";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        worksheet.Range(1, 1, 1, 13).Merge();

        worksheet.Cell(2, 1).Value = "1. Complete todos los campos obligatorios";
        worksheet.Cell(3, 1).Value = "2. Las fechas deben estar en formato: DD/MM/YYYY";
        worksheet.Cell(4, 1).Value = "3. Use las listas desplegables para Institución, Corte de Datos y Guía (ver hojas de referencia)";
        worksheet.Cell(5, 1).Value = "4. Se validará que no exista una evaluación duplicada (mismo paciente, guía y fecha)";
        worksheet.Cell(6, 1).Value = "5. Si el paciente o funcionario no existe, se creará automáticamente";

        // Espacio vacío
        worksheet.Cell(8, 1).Value = "DATOS DE EVALUACIONES";
        worksheet.Cell(8, 1).Style.Font.Bold = true;
        worksheet.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        worksheet.Range(8, 1, 8, 8).Merge();

        // Headers para las evaluaciones
        for (int i = 0; i < ColumnHeaders.Length; i++)
        {
            worksheet.Cell(9, i + 1).Value = ColumnHeaders[i];
        }

        var headerRange = worksheet.Range(9, 1, 9, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.WrapText = true;

        // Set column widths
        worksheet.Column(1).Width = 25;  // Identificación Paciente
        worksheet.Column(2).Width = 25;  // Identificación Funcionario
        worksheet.Column(3).Width = 30;  // Institución
        worksheet.Column(4).Width = 25;  // Corte de Datos
        worksheet.Column(5).Width = 35;  // Guía
        worksheet.Column(6).Width = 18;  // Fecha Evaluación
        worksheet.Column(7).Width = 10;  // Edad
        worksheet.Column(8).Width = 20;  // EPS

        // Add example data
        worksheet.Cell(10, 1).Value = "1234567890";
        worksheet.Cell(10, 2).Value = "9876543210";
        worksheet.Cell(10, 3).Value = "Hospital Central";
        worksheet.Cell(10, 4).Value = "Corte 2024-Q1";
        worksheet.Cell(10, 5).Value = "Guía de Ejemplo";
        worksheet.Cell(10, 6).Value = DateTime.Now;
        worksheet.Cell(10, 7).Value = "34";
        worksheet.Cell(10, 8).Value = "EPS Salud";

        // Format date columns
        worksheet.Column(6).Style.DateFormat.Format = "dd/mm/yyyy";
    }

    /// <summary>
    /// Crea una plantilla Excel con contexto específico (institución, corte de datos y guía como parámetros)
    /// Muestra las preguntas de la guía seleccionada con opciones de escala
    /// </summary>
    public async Task<IXLWorkbook> CreateTemplateWithContextAsync(AppDbContext dbContext, int institutionId, int dataCutId, int guideId)
    {
        // Obtener entidades del contexto
        var institution = await dbContext.Institutions
            .FirstOrDefaultAsync(i => i.Id == institutionId);

        var dataCut = await dbContext.DataCuts
            .FirstOrDefaultAsync(dc => dc.Id == dataCutId);

        var guide = await dbContext.Guides
            .Include(g => g.Questions)
            .Include(g => g.Scale)
                .ThenInclude(s => s.Equivalences)
            .FirstOrDefaultAsync(g => g.Id == guideId);

        if (institution == null || dataCut == null || guide == null)
        {
            throw new ArgumentException("Institución, Corte de Datos o Guía no encontrados");
        }

        var workbook = new XLWorkbook();
        var mainSheet = workbook.Worksheets.Add("Evaluaciones");

        // Formatear la hoja con contexto específico
        FormatMainSheetWithContext(mainSheet, institution, dataCut, guide);

        // Crear hoja de referencia con opciones de escala
        if (guide.Scale != null)
        {
            CreateScaleOptionsSheet(workbook, guide.Scale);
        }

        return workbook;
    }

    /// <summary>
    /// Crea una plantilla Excel con datos de referencia de la base de datos
    /// </summary>
    public IXLWorkbook CreateTemplateWithData(AppDbContext dbContext)
    {
        var workbook = new XLWorkbook();

        // Hoja principal de datos
        var mainSheet = workbook.Worksheets.Add("Evaluaciones");
        FormatMainSheet(mainSheet);

        // Obtener datos de referencia
        var institutions = dbContext.Institutions.OrderBy(i => i.Name).ToList();
        var dataCuts = dbContext.DataCuts.Include(dc => dc.Institution).OrderBy(dc => dc.Name).ToList();
        var guides = dbContext.Guides.OrderBy(g => g.Name).ToList();

        // Crear hojas de referencia
        CreateInstitutionsSheet(workbook, institutions);
        CreateDataCutsSheet(workbook, dataCuts);
        CreateGuidesSheet(workbook, guides);

        // Aplicar validaciones con listas desplegables
        ApplyDataValidations(mainSheet, institutions, dataCuts, guides);

        return workbook;
    }

    /// <summary>
    /// Formatea la hoja principal de evaluaciones
    /// </summary>
    private void FormatMainSheet(IXLWorksheet worksheet)
    {
        // Información de instrucciones
        worksheet.Cell(1, 1).Value = "INSTRUCCIONES DE IMPORTACIÓN DE EVALUACIONES";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        worksheet.Range(1, 1, 1, 8).Merge();

        worksheet.Cell(2, 1).Value = "1. Complete todos los campos obligatorios";
        worksheet.Cell(3, 1).Value = "2. Las fechas deben estar en formato: DD/MM/YYYY";
        worksheet.Cell(4, 1).Value = "3. Use las listas desplegables para Institución, Corte de Datos y Guía";
        worksheet.Cell(5, 1).Value = "4. Consulte las hojas 'Instituciones', 'CorteDatos' y 'Guías' para ver los datos disponibles";
        worksheet.Cell(6, 1).Value = "5. Si el paciente o funcionario no existe, se creará automáticamente";

        // Encabezado de datos
        worksheet.Cell(8, 1).Value = "DATOS DE EVALUACIONES";
        worksheet.Cell(8, 1).Style.Font.Bold = true;
        worksheet.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        worksheet.Range(8, 1, 8, 8).Merge();

        // Headers de columnas
        var headers = new[] {
            "Identificación Paciente",
            "Identificación Funcionario",
            "Institución", "Corte de Datos", "Guía",
            "Fecha Evaluación", "Edad", "EPS"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(9, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            cell.Style.Alignment.WrapText = true;
        }

        // Anchos de columna
        worksheet.Column(1).Width = 25;
        worksheet.Column(2).Width = 25;
        worksheet.Column(3).Width = 30;
        worksheet.Column(4).Width = 25;
        worksheet.Column(5).Width = 35;
        worksheet.Column(6).Width = 18;
        worksheet.Column(7).Width = 10;
        worksheet.Column(8).Width = 20;

        // Formato de fechas
        worksheet.Column(6).Style.DateFormat.Format = "dd/mm/yyyy";
    }

    /// <summary>
    /// Crea la hoja de referencia de Instituciones
    /// </summary>
    private void CreateInstitutionsSheet(IXLWorkbook workbook, List<Institution> institutions)
    {
        var sheet = workbook.Worksheets.Add("Instituciones");

        sheet.Cell(1, 1).Value = "Nombre";
        sheet.Cell(1, 2).Value = "Abreviación";
        sheet.Cell(1, 3).Value = "Ciudad";

        var headerRange = sheet.Range(1, 1, 1, 3);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        int row = 2;
        foreach (var institution in institutions)
        {
            sheet.Cell(row, 1).Value = institution.Name;
            sheet.Cell(row, 2).Value = institution.Abbreviation;
            sheet.Cell(row, 3).Value = institution.City;
            row++;
        }

        sheet.Column(1).Width = 40;
        sheet.Column(2).Width = 20;
        sheet.Column(3).Width = 25;
    }

    /// <summary>
    /// Crea la hoja de referencia de Cortes de Datos
    /// </summary>
    private void CreateDataCutsSheet(IXLWorkbook workbook, List<DataCut> dataCuts)
    {
        var sheet = workbook.Worksheets.Add("CorteDatos");

        sheet.Cell(1, 1).Value = "Nombre";
        sheet.Cell(1, 2).Value = "Institución";
        sheet.Cell(1, 3).Value = "Ciclo";
        sheet.Cell(1, 4).Value = "Fecha Inicial";
        sheet.Cell(1, 5).Value = "Fecha Final";

        var headerRange = sheet.Range(1, 1, 1, 5);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        int row = 2;
        foreach (var dataCut in dataCuts)
        {
            sheet.Cell(row, 1).Value = dataCut.Name;
            sheet.Cell(row, 2).Value = dataCut.Institution?.Name ?? "";
            sheet.Cell(row, 3).Value = dataCut.Cycle;
            sheet.Cell(row, 4).Value = dataCut.InitialDate;
            sheet.Cell(row, 5).Value = dataCut.FinalDate;
            row++;
        }

        sheet.Column(1).Width = 30;
        sheet.Column(2).Width = 35;
        sheet.Column(3).Width = 15;
        sheet.Column(4).Width = 15;
        sheet.Column(5).Width = 15;

        sheet.Column(4).Style.DateFormat.Format = "dd/mm/yyyy";
        sheet.Column(5).Style.DateFormat.Format = "dd/mm/yyyy";
    }

    /// <summary>
    /// Crea la hoja de referencia de Guías
    /// </summary>
    private void CreateGuidesSheet(IXLWorkbook workbook, List<Guide> guides)
    {
        var sheet = workbook.Worksheets.Add("Guías");

        sheet.Cell(1, 1).Value = "Nombre";
        sheet.Cell(1, 2).Value = "Descripción";

        var headerRange = sheet.Range(1, 1, 1, 2);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        int row = 2;
        foreach (var guide in guides)
        {
            sheet.Cell(row, 1).Value = guide.Name;
            sheet.Cell(row, 2).Value = guide.Description;
            row++;
        }

        sheet.Column(1).Width = 40;
        sheet.Column(2).Width = 60;
    }

    /// <summary>
    /// Formatea la hoja principal con contexto específico (institución, corte y guía como parámetros)
    /// </summary>
    private void FormatMainSheetWithContext(IXLWorksheet worksheet, Institution institution, DataCut dataCut, Guide guide)
    {
        // Información de contexto (oculta en columnas especiales)
        worksheet.Cell(1, 1).Value = "CONTEXTO_INSTITUCION_ID";
        worksheet.Cell(1, 2).Value = institution.Id;
        worksheet.Cell(2, 1).Value = "CONTEXTO_DATACURT_ID";
        worksheet.Cell(2, 2).Value = dataCut.Id;
        worksheet.Cell(3, 1).Value = "CONTEXTO_GUIA_ID";
        worksheet.Cell(3, 2).Value = guide.Id;

        // Ocultar las filas de contexto
        worksheet.Row(1).Hide();
        worksheet.Row(2).Hide();
        worksheet.Row(3).Hide();

        // Encabezado visible con información de contexto
        worksheet.Cell(5, 1).Value = $"PLANTILLA DE IMPORTACIÓN DE EVALUACIONES";
        worksheet.Cell(5, 1).Style.Font.Bold = true;
        worksheet.Cell(5, 1).Style.Font.FontSize = 14;
        worksheet.Cell(5, 1).Style.Fill.BackgroundColor = XLColor.DarkBlue;
        worksheet.Cell(5, 1).Style.Font.FontColor = XLColor.White;

        var questionsCount = guide.Questions?.Count ?? 0;
        var totalColumns = 5 + questionsCount;
        worksheet.Range(5, 1, 5, totalColumns).Merge();

        // Información de contexto visible
        worksheet.Cell(7, 1).Value = "Institución:";
        worksheet.Cell(7, 1).Style.Font.Bold = true;
        worksheet.Cell(7, 2).Value = institution.Name;
        worksheet.Cell(7, 2).Style.Font.Bold = true;
        worksheet.Cell(7, 2).Style.Font.FontColor = XLColor.DarkBlue;

        worksheet.Cell(8, 1).Value = "Corte de Datos:";
        worksheet.Cell(8, 1).Style.Font.Bold = true;
        worksheet.Cell(8, 2).Value = dataCut.Name;
        worksheet.Cell(8, 2).Style.Font.Bold = true;
        worksheet.Cell(8, 2).Style.Font.FontColor = XLColor.DarkBlue;

        worksheet.Cell(9, 1).Value = "Guía:";
        worksheet.Cell(9, 1).Style.Font.Bold = true;
        worksheet.Cell(9, 2).Value = guide.Name;
        worksheet.Cell(9, 2).Style.Font.Bold = true;
        worksheet.Cell(9, 2).Style.Font.FontColor = XLColor.DarkBlue;

        worksheet.Cell(10, 1).Value = "Escala:";
        worksheet.Cell(10, 1).Style.Font.Bold = true;
        worksheet.Cell(10, 2).Value = guide.Scale?.Name ?? "Sin escala";
        worksheet.Cell(10, 2).Style.Font.Bold = true;
        worksheet.Cell(10, 2).Style.Font.FontColor = XLColor.DarkBlue;

        // Instrucciones
        worksheet.Cell(12, 1).Value = "INSTRUCCIONES:";
        worksheet.Cell(12, 1).Style.Font.Bold = true;
        worksheet.Cell(13, 1).Value = "1. Complete la identificación del paciente y funcionario";
        worksheet.Cell(14, 1).Value = "2. Ingrese la fecha de evaluación en formato DD/MM/YYYY";
        worksheet.Cell(15, 1).Value = "3. Para cada pregunta, seleccione una opción de la lista desplegable (ver hoja 'Opciones de Escala')";
        worksheet.Cell(16, 1).Value = "4. No modifique las filas ocultas ni la información de contexto";

        // Encabezado de datos
        worksheet.Cell(18, 1).Value = "DATOS DE EVALUACIONES";
        worksheet.Cell(18, 1).Style.Font.Bold = true;
        worksheet.Cell(18, 1).Style.Fill.BackgroundColor = XLColor.DarkGray;
        worksheet.Cell(18, 1).Style.Font.FontColor = XLColor.White;
        worksheet.Range(18, 1, 18, totalColumns).Merge();

        // Headers de columnas base
        var baseHeaders = new[] {
            "Identificación Paciente",
            "Identificación Funcionario",
            "Fecha Evaluación",
            "Edad",
            "EPS"
        };

        int col = 1;
        foreach (var header in baseHeaders)
        {
            var cell = worksheet.Cell(19, col);
            cell.Value = header;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            cell.Style.Alignment.WrapText = true;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            col++;
        }

        // Headers de preguntas dinámicas
        if (guide.Questions != null && guide.Questions.Any())
        {
            foreach (var question in guide.Questions.OrderBy(q => q.Id))
            {
                var cell = worksheet.Cell(19, col);
                cell.Value = $"P{question.Id}: {question.Text}";
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                cell.Style.Alignment.WrapText = true;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Ancho de columna ajustado
                worksheet.Column(col).Width = 35;

                col++;
            }
        }

        // Anchos de columnas base
        worksheet.Column(1).Width = 25;  // Identificación Paciente
        worksheet.Column(2).Width = 25;  // Identificación Funcionario
        worksheet.Column(3).Width = 18;  // Fecha Evaluación
        worksheet.Column(4).Width = 10;  // Edad
        worksheet.Column(5).Width = 20;  // EPS

        // Formato de fechas
        worksheet.Column(3).Style.DateFormat.Format = "dd/mm/yyyy";

        // Aplicar validaciones de escala en las columnas de preguntas
        if (guide.Scale != null && guide.Scale.Equivalences != null && guide.Scale.Equivalences.Any())
        {
            ApplyScaleValidations(worksheet, guide);
        }
    }

    /// <summary>
    /// Crea la hoja de referencia con las opciones de escala
    /// </summary>
    private void CreateScaleOptionsSheet(IXLWorkbook workbook, Scale scale)
    {
        var sheet = workbook.Worksheets.Add("Opciones de Escala");

        sheet.Cell(1, 1).Value = $"Opciones de la Escala: {scale.Name}";
        sheet.Cell(1, 1).Style.Font.Bold = true;
        sheet.Cell(1, 1).Style.Font.FontSize = 12;
        sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
        sheet.Range(1, 1, 1, 3).Merge();

        sheet.Cell(3, 1).Value = "Nombre";
        sheet.Cell(3, 2).Value = "Valor";
        sheet.Cell(3, 3).Value = "Descripción";

        var headerRange = sheet.Range(3, 1, 3, 3);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 4;
        if (scale.Equivalences != null)
        {
            foreach (var equivalence in scale.Equivalences.OrderBy(e => e.Value))
            {
                sheet.Cell(row, 1).Value = equivalence.Name;
                sheet.Cell(row, 2).Value = equivalence.Value;
                sheet.Cell(row, 3).Value = $"Valor: {equivalence.Value}";
                row++;
            }
        }

        sheet.Column(1).Width = 30;
        sheet.Column(2).Width = 15;
        sheet.Column(3).Width = 50;
    }

    /// <summary>
    /// Aplica validaciones de escala en las columnas de preguntas
    /// </summary>
    private void ApplyScaleValidations(IXLWorksheet worksheet, Guide guide)
    {
        if (guide.Questions == null || !guide.Questions.Any()) return;
        if (guide.Scale?.Equivalences == null || !guide.Scale.Equivalences.Any()) return;

        var equivalenceCount = guide.Scale.Equivalences.Count;
        int startCol = 6; // Comienza después de las 5 columnas base

        foreach (var question in guide.Questions.OrderBy(q => q.Id))
        {
            var questionRange = worksheet.Range($"{GetColumnLetter(startCol)}20:{GetColumnLetter(startCol)}1000");
            var validation = questionRange.CreateDataValidation();
            validation.List($"'Opciones de Escala'!$A$4:$A${equivalenceCount + 3}", true);
            startCol++;
        }
    }

    /// <summary>
    /// Convierte un índice de columna (1-based) a letra de Excel
    /// </summary>
    private string GetColumnLetter(int columnNumber)
    {
        string columnName = "";

        while (columnNumber > 0)
        {
            int modulo = (columnNumber - 1) % 26;
            columnName = Convert.ToChar('A' + modulo) + columnName;
            columnNumber = (columnNumber - modulo) / 26;
        }

        return columnName;
    }

    /// <summary>
    /// Aplica validaciones de datos con listas desplegables
    /// </summary>
    private void ApplyDataValidations(IXLWorksheet mainSheet, List<Institution> institutions, List<DataCut> dataCuts, List<Guide> guides)
    {
        // Validación para Institución (columna 3)
        if (institutions.Any())
        {
            var institutionRange = mainSheet.Range($"C10:C1000");
            var validation = institutionRange.CreateDataValidation();
            validation.List($"Instituciones!$A$2:$A${institutions.Count + 1}", true);
        }

        // Validación para Corte de Datos (columna 4)
        if (dataCuts.Any())
        {
            var dataCutRange = mainSheet.Range($"D10:D1000");
            var validation = dataCutRange.CreateDataValidation();
            validation.List($"CorteDatos!$A$2:$A${dataCuts.Count + 1}", true);
        }

        // Validación para Guía (columna 5)
        if (guides.Any())
        {
            var guideRange = mainSheet.Range($"E10:E1000");
            var validation = guideRange.CreateDataValidation();
            validation.List($"Guías!$A$2:$A${guides.Count + 1}", true);
        }
    }

    /// <summary>
    /// Importa evaluaciones desde Excel con contexto específico (institución, corte y guía como parámetros)
    /// </summary>
    public async Task<ImportResult<Assessment>> ImportAssessmentsWithContextAsync(
        IFormFile file,
        AppDbContext dbContext,
        int userId,
        int institutionId,
        int dataCutId,
        int guideId)
    {
        var importedAssessments = new List<Assessment>();
        var duplicateRows = new List<string>();
        var errorRows = new List<string>();
        int totalProcessed = 0;

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            // Leer los IDs de contexto de las filas ocultas
            var contextInstitutionId = worksheet.Cell(1, 2).GetValue<int>();
            var contextDataCutId = worksheet.Cell(2, 2).GetValue<int>();
            var contextGuideId = worksheet.Cell(3, 2).GetValue<int>();

            // Validar que los IDs de contexto coincidan con los parámetros
            if (contextInstitutionId != institutionId || contextDataCutId != dataCutId || contextGuideId != guideId)
            {
                errorRows.Add("Los IDs de contexto en el archivo no coinciden con los parámetros proporcionados");
                return new ImportResult<Assessment>(importedAssessments, duplicateRows, errorRows, 0, "Error de validación de contexto");
            }

            // Obtener la guía con sus preguntas y escala
            var guide = await dbContext.Guides
                .Include(g => g.Questions)
                .Include(g => g.Scale)
                    .ThenInclude(s => s.Equivalences)
                .FirstOrDefaultAsync(g => g.Id == guideId);

            if (guide == null)
            {
                errorRows.Add("La guía especificada no existe");
                return new ImportResult<Assessment>(importedAssessments, duplicateRows, errorRows, 0, "Guía no encontrada");
            }

            var rowCount = worksheet.RowsUsed().Count() + 4; // +4 to account of espace in template

            // Comenzar desde la fila 20 (después de las instrucciones y headers)
            for (int row = 20; row <= rowCount; row++)
            {
                totalProcessed++;
                try
                {
                    var model = CreateEntityFromRowWithContext(worksheet.Row(row), guide);
                    if (model == null)
                    {
                        errorRows.Add($"Fila {row}: Datos vacíos o inválidos");
                        continue;
                    }

                    // Establecer el UserId del importador
                    var modelWithUser = model with { UserId = userId };

                    // Validar y crear/obtener entidades relacionadas
                    var validationResult = await ValidateAndProcessAssessmentAsync(
                        modelWithUser,
                        dbContext,
                        row,
                        institutionId,
                        dataCutId,
                        guideId);

                    if (!validationResult.IsSuccess)
                    {
                        if (validationResult.IsDuplicate)
                        {
                            duplicateRows.Add(validationResult.ErrorMessage);
                        }
                        else
                        {
                            errorRows.Add(validationResult.ErrorMessage);
                        }
                        continue;
                    }

                    // Crear las valoraciones (valuations) para cada pregunta
                    var assessment = validationResult.Assessment!;
                    if (model.Valuations != null && model.Valuations.Any())
                    {
                        await CreateValuationsAsync(assessment, model.Valuations, guide, dbContext);
                    }

                    importedAssessments.Add(assessment);
                }
                catch (Exception ex)
                {
                    errorRows.Add($"Fila {row}: {ex.Message}");
                }
            }

            // Guardar todas las evaluaciones en una transacción
            if (importedAssessments.Any())
            {
                try
                {
                    using var transaction = await dbContext.Database.BeginTransactionAsync();
                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    errorRows.Add($"Error al guardar en base de datos: {ex.Message}");
                    importedAssessments.Clear();
                }
            }

            var summary = $"Importación terminada. Correctamente importadas: {importedAssessments.Count}";
            if (duplicateRows.Count > 0)
            {
                summary += $", Duplicados omitidos: {duplicateRows.Count}";
            }
            if (errorRows.Count > 0)
            {
                summary += $", Errores: {errorRows.Count}";
            }

            return new ImportResult<Assessment>(importedAssessments, duplicateRows, errorRows, totalProcessed, summary);
        }
        catch (Exception ex)
        {
            errorRows.Add($"Error general: {ex.Message}");
            return new ImportResult<Assessment>(importedAssessments, duplicateRows, errorRows, totalProcessed, $"Importación fallida: {ex.Message}");
        }
    }

    /// <summary>
    /// Importa evaluaciones desde Excel con validaciones completas (método legacy)
    /// </summary>
    public async Task<ImportResult<Assessment>> ImportAssessmentsAsync(IFormFile file, AppDbContext dbContext, int userId)
    {
        var importedAssessments = new List<Assessment>();
        var duplicateRows = new List<string>();
        var errorRows = new List<string>();
        int totalProcessed = 0;

        errorRows.Add("Este método está deprecado. Use ImportAssessmentsWithContextAsync en su lugar");

        return new ImportResult<Assessment>(importedAssessments, duplicateRows, errorRows, totalProcessed, "Método deprecado");
    }

    /// <summary>
    /// Crea un modelo de importación desde una fila de Excel con contexto específico
    /// </summary>
    private AssessmentImportModel? CreateEntityFromRowWithContext(IXLRow row, Guide guide)
    {
        var patientId = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(patientId)) return null; // Skip empty rows

        var model = new AssessmentImportModel
        {
            PatientIdentification = row.Cell(1).GetString(),
            FunctionaryIdentification = row.Cell(2).GetString(),
            AssessmentDate = row.Cell(3).GetDateTime(),
            YearOld = row.Cell(4).GetString(),
            Eps = row.Cell(5).GetString(),
            Valuations = new Dictionary<int, string>()
        };

        // Leer las valoraciones (una por cada pregunta)
        if (guide.Questions != null && guide.Questions.Any())
        {
            int col = 6; // Comienza en la columna 6 (después de las 5 columnas base)
            foreach (var question in guide.Questions.OrderBy(q => q.Id))
            {
                var equivalenceName = row.Cell(col).GetString();
                if (!string.IsNullOrWhiteSpace(equivalenceName))
                {
                    model.Valuations[question.Id] = equivalenceName;
                }
                col++;
            }
        }

        return model;
    }

    /// <summary>
    /// Crea las valoraciones para una evaluación
    /// </summary>
    private async Task CreateValuationsAsync(
        Assessment assessment,
        Dictionary<int, string> valuations,
        Guide guide,
        AppDbContext dbContext)
    {
        if (guide.Scale?.Equivalences == null || !guide.Scale.Equivalences.Any())
        {
            return;
        }

        int order = 1;
        foreach (var valuation in valuations)
        {
            var questionId = valuation.Key;
            var equivalenceName = valuation.Value;

            // Buscar la pregunta
            var question = guide.Questions?.FirstOrDefault(q => q.Id == questionId);
            if (question == null) continue;

            // Buscar la equivalencia por nombre
            var equivalence = guide.Scale.Equivalences
                .FirstOrDefault(e => e.Name.Equals(equivalenceName, StringComparison.OrdinalIgnoreCase));

            if (equivalence != null)
            {
                var newValuation = Valuation.Create(
                    id: 0,
                    order: order,
                    text: question.Text,
                    idEquivalence: equivalence.Id,
                    idAssessment: assessment.Id,
                    idUser: assessment.IdUserCreated,
                    idQuestion: questionId
                );

                assessment.Valuations.Add(newValuation);

                order++;
            }
        }
    }

    /// <summary>
    /// Valida y procesa una evaluación con contexto específico (institución, corte y guía ya definidos)
    /// </summary>
    private async Task<AssessmentValidationResult> ValidateAndProcessAssessmentAsync(
        AssessmentImportModel model,
        AppDbContext dbContext,
        int rowNumber,
        int institutionId,
        int dataCutId,
        int guideId)
    {
        try
        {
            // 1. Validar y obtener/crear Paciente
            var patient = await GetOrCreatePatientAsync(model, dbContext);
            if (patient == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: Error al crear/obtener el paciente");
            }

            // 2. Validar y obtener/crear Funcionario
            var functionary = await GetOrCreateFunctionaryAsync(model, dbContext);
            if (functionary == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: Error al crear/obtener el funcionario");
            }

            // 3. Obtener el corte de datos para validar la fecha
            var dataCut = await dbContext.DataCuts.FindAsync(dataCutId);
            if (dataCut == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: El corte de datos no existe");
            }

            // 4. Validar que la fecha de evaluación esté dentro del rango del corte de datos
            // if (model.AssessmentDate < dataCut.InitialDate || model.AssessmentDate > dataCut.FinalDate)
            // {
            //     return AssessmentValidationResult.Error($"Fila {rowNumber}: La fecha de evaluación {model.AssessmentDate:dd/MM/yyyy} está fuera del rango del corte de datos ({dataCut.InitialDate:dd/MM/yyyy} - {dataCut.FinalDate:dd/MM/yyyy})");
            // }

            // 5. Verificar que no exista una evaluación duplicada
            var existingAssessment = await dbContext.Assessments
                .AnyAsync(a => a.PatientId == patient.Id
                    && a.GuideId == guideId
                    && a.Date.Date == model.AssessmentDate.Date);

            if (existingAssessment)
            {
                return AssessmentValidationResult.Duplicate($"Fila {rowNumber}: Ya existe una evaluación para el paciente con identificación '{model.PatientIdentification}' en la fecha {model.AssessmentDate:dd/MM/yyyy}");
            }

            // 6. Crear la evaluación
            var assessment = Assessment.Create(
                id: 0,
                idInstitucion: institutionId,
                idDataCut: dataCutId,
                idFunctionary: functionary.Id,
                idPatient: patient.Id,
                yearOld: model.YearOld,
                date: model.AssessmentDate,
                eps: model.Eps,
                idUser: model.UserId,
                idGuide: guideId
            );

            dbContext.Assessments.Add(assessment);

            return AssessmentValidationResult.Success(assessment);
        }
        catch (Exception ex)
        {
            return AssessmentValidationResult.Error($"Fila {rowNumber}: Error al procesar: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene un paciente existente o crea uno nuevo
    /// </summary>
    private async Task<Patient?> GetOrCreatePatientAsync(AssessmentImportModel model, AppDbContext dbContext)
    {
        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(p => p.Identification == model.PatientIdentification);

        if (patient == null)
        {
            patient = Patient.Create(
                id: 0,
                firstName: "",
                lastName: "",
                identification: model.PatientIdentification,
                birthDate: DateTime.Now,
                eps: model.Eps
            );
            dbContext.Patients.Add(patient);
        }
        else
        {
            // Actualizar información si es necesaria
            patient.Update(
                firstName: patient.FirstName ?? "",
                lastName: patient.LastName ?? "",
                identification: model.PatientIdentification,
                birthDate: patient.BirthDate,
                eps: model.Eps
            );
        }

        return patient;
    }

    /// <summary>
    /// Obtiene un funcionario existente o crea uno nuevo
    /// </summary>
    private async Task<Functionary?> GetOrCreateFunctionaryAsync(AssessmentImportModel model, AppDbContext dbContext)
    {
        var functionary = await dbContext.Functionaries
            .FirstOrDefaultAsync(f => f.Identification == model.FunctionaryIdentification);

        if (functionary == null)
        {
            functionary = Functionary.Create(
                id: 0,
                firstName: "",
                lastName: "",
                identification: model.FunctionaryIdentification
            );
            dbContext.Functionaries.Add(functionary);
        }
        else
        {
            // Actualizar información si es necesaria (mantener los nombres existentes)
            functionary.Update(
                firstName: functionary.FirstName,
                lastName: functionary.LastName,
                identification: model.FunctionaryIdentification
            );
        }

        return functionary;
    }

    /// <summary>
    /// Resultado de la validación de una evaluación
    /// </summary>
    private class AssessmentValidationResult
    {
        public bool IsSuccess { get; private set; }
        public bool IsDuplicate { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public Assessment? Assessment { get; private set; }

        public static AssessmentValidationResult Success(Assessment assessment)
        {
            return new AssessmentValidationResult
            {
                IsSuccess = true,
                IsDuplicate = false,
                Assessment = assessment
            };
        }

        public static AssessmentValidationResult Error(string message)
        {
            return new AssessmentValidationResult
            {
                IsSuccess = false,
                IsDuplicate = false,
                ErrorMessage = message
            };
        }

        public static AssessmentValidationResult Duplicate(string message)
        {
            return new AssessmentValidationResult
            {
                IsSuccess = false,
                IsDuplicate = true,
                ErrorMessage = message
            };
        }
    }
}

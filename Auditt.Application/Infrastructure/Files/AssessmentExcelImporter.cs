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
        var patientId = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(patientId)) return null; // Skip empty rows

        return new AssessmentImportModel
        {
            PatientIdentification = row.Cell(1).GetString(),
            FunctionaryIdentification = row.Cell(2).GetString(),
            InstitutionName = row.Cell(3).GetString(),
            DataCutName = row.Cell(4).GetString(),
            GuideName = row.Cell(5).GetString(),
            AssessmentDate = row.Cell(6).GetDateTime(),
            YearOld = row.Cell(7).GetString(),
            Eps = row.Cell(8).GetString()
        };
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
    /// Importa evaluaciones desde Excel con validaciones completas
    /// </summary>
    public async Task<ImportResult<Assessment>> ImportAssessmentsAsync(IFormFile file, AppDbContext dbContext, int userId)
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
            var rowCount = worksheet.RowsUsed().Count();

            // Comenzar desde la fila 10 (después de las instrucciones y headers)
            for (int row = 10; row <= rowCount; row++)
            {
                totalProcessed++;
                try
                {
                    var model = CreateEntityFromRow(worksheet.Row(row));
                    if (model == null)
                    {
                        errorRows.Add($"Fila {row}: Datos vacíos o inválidos");
                        continue;
                    }

                    // Establecer el UserId del importador
                    var modelWithUser = model with { UserId = userId };

                    // Validar y crear/obtener entidades relacionadas
                    var validationResult = await ValidateAndProcessAssessmentAsync(modelWithUser, dbContext, row);

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

                    importedAssessments.Add(validationResult.Assessment!);
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
    /// Valida y procesa una evaluación, creando o reutilizando entidades relacionadas
    /// </summary>
    private async Task<AssessmentValidationResult> ValidateAndProcessAssessmentAsync(
        AssessmentImportModel model,
        AppDbContext dbContext,
        int rowNumber)
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

            // 3. Validar y obtener Institución (debe existir)
            var institution = await dbContext.Institutions
                .FirstOrDefaultAsync(i => i.Name.ToUpper() == model.InstitutionName.ToUpper());
            if (institution == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: La institución '{model.InstitutionName}' no existe en el sistema");
            }

            // 4. Validar y obtener Corte de Datos (debe existir y pertenecer a la institución)
            var dataCut = await dbContext.DataCuts
                .FirstOrDefaultAsync(dc => dc.Name.ToUpper() == model.DataCutName.ToUpper()
                    && dc.InstitutionId == institution.Id);
            if (dataCut == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: El corte de datos '{model.DataCutName}' no existe para la institución '{model.InstitutionName}'");
            }

            // 5. Validar que la fecha de evaluación esté dentro del rango del corte de datos
            if (model.AssessmentDate < dataCut.InitialDate || model.AssessmentDate > dataCut.FinalDate)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: La fecha de evaluación {model.AssessmentDate:dd/MM/yyyy} está fuera del rango del corte de datos ({dataCut.InitialDate:dd/MM/yyyy} - {dataCut.FinalDate:dd/MM/yyyy})");
            }

            // 6. Validar y obtener Guía (debe existir)
            var guide = await dbContext.Guides
                .FirstOrDefaultAsync(g => g.Name.ToUpper() == model.GuideName.ToUpper());
            if (guide == null)
            {
                return AssessmentValidationResult.Error($"Fila {rowNumber}: La guía '{model.GuideName}' no existe en el sistema");
            }

            // 7. Verificar que no exista una evaluación duplicada
            // (mismo paciente, misma guía, misma fecha)
            var existingAssessment = await dbContext.Assessments
                .AnyAsync(a => a.PatientId == patient.Id
                    && a.GuideId == guide.Id
                    && a.Date.Date == model.AssessmentDate.Date);

            if (existingAssessment)
            {
                return AssessmentValidationResult.Duplicate($"Fila {rowNumber}: Ya existe una evaluación para el paciente con identificación '{model.PatientIdentification}' con la guía '{model.GuideName}' en la fecha {model.AssessmentDate:dd/MM/yyyy}");
            }

            // 8. Crear la evaluación
            var assessment = Assessment.Create(
                id: 0,
                idInstitucion: institution.Id,
                idDataCut: dataCut.Id,
                idFunctionary: functionary.Id,
                idPatient: patient.Id,
                yearOld: model.YearOld,
                date: model.AssessmentDate,
                eps: model.Eps,
                idUser: model.UserId,
                idGuide: guide.Id
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

using Auditt.Application.Domain.Entities;
using Auditt.Application.Domain.Models;
using Auditt.Application.Infrastructure.Sqlite;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Auditt.Application.Infrastructure.Files;

public class GuideExcelImporter : ExcelImporter<QuestionImportModel>
{
    protected override string[] ColumnHeaders => new[]
    {
        "QuestionText",
        "QuestionOrder"
    };

    protected override QuestionImportModel? CreateEntityFromRow(IXLRow row)
    {
        var questionText = row.Cell(1).GetString();
        if (string.IsNullOrWhiteSpace(questionText)) return null; // Skip empty rows

        return new QuestionImportModel
        {
            Text = questionText,
            Order = (int)row.Cell(2).GetDouble()
        };
    }

    protected override bool EntityExists(QuestionImportModel entity, AppDbContext dbContext)
    {
        // No validamos duplicados de preguntas aquí
        return false;
    }

    protected override void FormatTemplate(IXLWorksheet worksheet)
    {
        // Información de la guía en las primeras filas
        worksheet.Cell(1, 1).Value = "Guide Information";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

        worksheet.Cell(2, 1).Value = "Name:";
        worksheet.Cell(2, 2).Value = "Nombre de la Guía";
        worksheet.Cell(3, 1).Value = "Description:";
        worksheet.Cell(3, 2).Value = "Descripción de la guía";

        // Espacio vacío
        worksheet.Cell(6, 1).Value = "Questions";
        worksheet.Cell(6, 1).Style.Font.Bold = true;
        worksheet.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

        // Headers para las preguntas
        worksheet.Cell(7, 1).Value = "QuestionText";
        worksheet.Cell(7, 2).Value = "QuestionOrder";

        var headerRange = worksheet.Range(7, 1, 7, 2);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Set column widths
        worksheet.Column(1).Width = 60; // QuestionText
        worksheet.Column(2).Width = 15; // QuestionOrder

        // Add example questions
        worksheet.Cell(8, 1).Value = "¿Pregunta de ejemplo número 1?";
        worksheet.Cell(8, 2).Value = 1;
        worksheet.Cell(9, 1).Value = "¿Pregunta de ejemplo número 2?";
        worksheet.Cell(9, 2).Value = 2;
        worksheet.Cell(10, 1).Value = "¿Pregunta de ejemplo número 3?";
        worksheet.Cell(10, 2).Value = 3;
    }

    public async Task<ImportResult<Guide>> ImportSingleGuideWithQuestionsAsync(IFormFile file, AppDbContext dbContext)
    {
        var importedGuides = new List<Guide>();
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

            // Leer información de la guía desde las primeras filas
            var guideName = worksheet.Cell(2, 2).GetString();
            var guideDescription = worksheet.Cell(3, 2).GetString();
            var scaleName = "General";

            // Validar datos de la guía
            if (string.IsNullOrWhiteSpace(guideName))
            {
                errorRows.Add("Guide name is required in cell B2");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, "Importación fallida: Nombre de guía requerido");
            }

            if (string.IsNullOrWhiteSpace(scaleName))
            {
                errorRows.Add("Scale name is required in cell B4");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, "Importación fallida: Nombre de escala requerido");
            }

            // Verificar si la guía ya existe
            var existingGuide = await dbContext.Guides.FirstOrDefaultAsync(g => g.Name == guideName);
            if (existingGuide != null)
            {
                duplicateRows.Add($"Guide '{guideName}' already exists");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, "Importación fallida: La guía ya existe");
            }

            // Verificar si la escala existe
            var scale = await dbContext.Scales.FirstOrDefaultAsync(s => s.Name.ToUpper().Contains(scaleName.ToUpper()));
            if (scale == null)
            {
                errorRows.Add($"Scale '{scaleName}' not found");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, "Importación fallida: Escala no encontrada");
            }

            // Leer preguntas desde la fila 8 en adelante
            var questions = new List<QuestionImportModel>();
            var rowCount = worksheet.RowsUsed().Count() + 2; // +2 to account of espace in template

            for (int row = 8; row <= rowCount; row++) // Start from 8 (questions section)
            {
                totalProcessed++;
                try
                {
                    var question = CreateEntityFromRow(worksheet.Row(row));
                    if (question != null)
                    {
                        questions.Add(question);
                    }
                }
                catch (Exception ex)
                {
                    errorRows.Add($"Row {row}: {ex.Message}");
                }
            }

            if (!questions.Any())
            {
                errorRows.Add("No valid questions found");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, "Importación fallida: No se encontraron preguntas válidas");
            }

            // Crear la guía y sus preguntas
            try
            {
                using var transaction = await dbContext.Database.BeginTransactionAsync();

                // Crear la guía
                var guide = Guide.Create(0, guideName, guideDescription, scale.Id);
                dbContext.Guides.Add(guide);
                await dbContext.SaveChangesAsync(); // Save to get guide ID

                // Crear las preguntas
                foreach (var questionModel in questions.OrderBy(q => q.Order))
                {
                    var question = Question.Create(0, questionModel.Text, questionModel.Order, guide.Id);
                    dbContext.Questions.Add(question);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                importedGuides.Add(guide);
            }
            catch (Exception ex)
            {
                errorRows.Add($"Database error: {ex.Message}");
                return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, $"Importación fallida: {ex.Message}");
            }

            var summary = $"Guía '{guideName}' importada exitosamente con {questions.Count} preguntas";
            return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, summary);
        }
        catch (Exception ex)
        {
            errorRows.Add($"Error general: {ex.Message}");
            return new ImportResult<Guide>(importedGuides, duplicateRows, errorRows, totalProcessed, $"Importación fallida: {ex.Message}");
        }
    }
}

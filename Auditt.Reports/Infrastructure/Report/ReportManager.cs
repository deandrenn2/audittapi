using ClosedXML.Report;
using Microsoft.AspNetCore.Hosting;

namespace Auditt.Reports.Infrastructure.Report;
public class ClosedXmlReportManager : IClosedXmlReportManager
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ClosedXmlReportManager(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public byte[] GenerateReportAsync<T>(string templateName, T data)
    {
        var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "template", $"{templateName}.xlsx");
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("Template file not found", templatePath);
        }

        using var stream = new MemoryStream();
        using var template = new XLTemplate(templatePath);

        template.AddVariable(data);
        template.Generate();

        template.Workbook.SaveAs(stream);
        return stream.ToArray();
    }
}

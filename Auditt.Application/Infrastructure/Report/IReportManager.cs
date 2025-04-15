namespace Auditt.Application.Infrastructure.Report;
public interface IClosedXmlReportManager
{
    byte[] GenerateReportAsync<T>(string templateName, T data);
}

using Auditt.Reports.Infrastructure.Report;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Auditt.Reports;

public static class DependencyConfig
{

    public static void AddInfraestructureReports(this IServiceCollection services)
    {

        services.AddScoped<IClosedXmlReportManager, ClosedXmlReportManager>();
    }


}
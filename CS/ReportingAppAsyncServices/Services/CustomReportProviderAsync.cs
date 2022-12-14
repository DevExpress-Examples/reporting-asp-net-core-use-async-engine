using System.IO;
using System.Threading.Tasks;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;

namespace ReportingAppAsyncServices.Services
{
    public class CustomReportProviderAsync : IReportProviderAsync
    {
        readonly ReportStorageWebExtension reportStorageWebExtension;

        public CustomReportProviderAsync(ReportStorageWebExtension reportStorageWebExtension)
        {
            this.reportStorageWebExtension = reportStorageWebExtension;
        }
        public async Task<XtraReport> GetReportAsync(string id, ReportProviderContext context)
        {
            var reportLayout = await reportStorageWebExtension.GetDataAsync(id);
            if (reportLayout == null)
                return null;
            using (var ms = new MemoryStream(reportLayout))
            {
                var report = XtraReport.FromXmlStream(ms);
                return report;
            }
        }
    }
}

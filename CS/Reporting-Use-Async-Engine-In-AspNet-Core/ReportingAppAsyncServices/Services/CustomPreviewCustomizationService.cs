using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingAppAsyncServices.Services
{
    public class CustomPreviewCustomizationService: PreviewReportCustomizationService
    {
        public override Task CustomizeReportAsync(XtraReport report)
        {
            //TODO: Call CreateDocumentAsync or do any async data retrieving
            return base.CustomizeReportAsync(report);
        }
    }
}

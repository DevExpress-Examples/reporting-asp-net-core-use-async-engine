using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;

namespace ReportingAppAsyncServices.Controllers {
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Designer([FromServices] IReportDesignerModelBuilder reportDesignerModelBuilder, [FromQuery] string reportName = "RootReport")
        {
            var dataSources = new Dictionary<string, object>
            {
                ["Northwind"] = GetNorthwindSqlDataSource()
            };
            var designerModel = await reportDesignerModelBuilder
                .DataSources(dataSources)
                .Report(reportName)
                .BuildModelAsync();
            return View(designerModel);
        }

        public async Task<IActionResult> Viewer([FromServices] IWebDocumentViewerClientSideModelGenerator modelGenerator, [FromQuery] string reportName = "RootReport")
        {
            var viewerModel = await modelGenerator.GetModelAsync(reportName, WebDocumentViewerController.DefaultUri);
            return View(viewerModel);
        }
        public async Task<IActionResult> ExportToPdf([FromServices] IReportProviderAsync reportProviderAsync, [FromQuery] string reportName = "RootReport")
        {
            var report = await reportProviderAsync.GetReportAsync(reportName, null);
            var reportServiceContainer = (IServiceContainer)report;
            reportServiceContainer.RemoveService(typeof(IReportProviderAsync));
            reportServiceContainer.AddService(typeof(IReportProviderAsync), reportProviderAsync);
            using (var stream = new MemoryStream()) {
                await report.CreateDocumentAsync();
                await report.ExportToPdfAsync(stream);
                return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
        }

        SqlDataSource GetNorthwindSqlDataSource()
        {
            // Create a SQL data source with the specified connection string.
            SqlDataSource ds = new SqlDataSource("NWindConnectionString");
            // Create a SQL query to access the Products data table.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            return ds;
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;

namespace ReportingAppAsyncServices.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Designer([FromQuery] string reportName = "RootReport")
        {
            var dataSources = new Dictionary<string, object>
            {
                ["Northwind"] = GetNorthwindSqlDataSource()
            };
            var modelGenerator = new ReportDesignerClientSideModelGenerator(HttpContext.RequestServices);
            var model = await modelGenerator.GetModelAsync(reportName, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            return View(model);
        }

        public async Task<IActionResult> Viewer([FromQuery] string reportName = "RootReport")
        {
            var modelGenerator = new WebDocumentViewerClientSideModelGenerator(HttpContext.RequestServices);
            var model = await modelGenerator.GetModelAsync(reportName, WebDocumentViewerController.DefaultUri);
            return View(model);
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

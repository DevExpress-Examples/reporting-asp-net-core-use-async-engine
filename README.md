<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/289745215/20.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T925249)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# How to Use the Asynchronous Engine for Web Reporting 
 
## Overview 
This example demonstrates how to use v20.2 services, which allow you to save, load, and export reports asynchronously. 
This application registers services with the **IReportProviderAsync** interface. The latter allows you to perform the following tasks asynchronously: 
- open subreports from the master report; 
- resolve a unique identifier to a report (an operation that affects the Designer Preview and Document Viewer performance). 

In general, asynchronous operation mode gives you the following advantages: 
- It handles threads in web applications more cautiously and returns threads to the thread pool while the operation is in progress.  
- You can write safe asynchronous code in a project with a third-party library that uses only asynchronous API. 
 
## Implementation Details 

### Perequisites 

To get you started, we created a sample project with our ASP.NET Core Reporting Project Template. 

### Report Provider 

We declared a new code unit - `CustomReportProviderAsync.cs` with a class that implements the **DevExpress.XtraReports.Services.IReportProviderAsync** interface and calls the report storage's **GetDataAsync** method. 

### Report Storage 

To implement a custom report storage with asynchronous methods, we declared a new code unit - `CustomReportStorageWebExtension.cs`. The CustomReportStorageWebExtension class inherits from the [ReportStorageWebExtension](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension) and overrides all its public methods. Note that methods, which cannot be used in asynchronous mode, throw exceptions.

### Report Model and Controllers

We used the **GetModelAsync** methods in the Home controller to generate report models and send them to the End User Report Designer and Document Viewer.  
ASP.NET Core Razor helpers cannot use asynchronous API if the Bind method receives a report instance or a string (ReportUrl) as a parameter. You must bind report controls to the [ReportDesignerModel](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Web.ReportDesigner.ReportDesignerModel) or [WebDocumentViewerModel](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Web.WebDocumentViewer.WebDocumentViewerModel) objects originated from controllers. The controller-based model allows you to use async API and avoid obscure problems. Such problems may occur when a subreport fails to load and throws an exception, or when a dynamic list of parameter values fails to retrieve its data.

### ExportToPdf Action 

The export action takes advantage of the **IReportProviderAsync** service that resolves report ID to a report and expedites the load of subreports without the need for the web report controls. We used the DI container to inject the **IReportProviderAsync** service into the XtraReport instance. Then, we can call the asynchronous **CreateDocumentAsync** and **ExportToPdfAsync** methods.

### Enable Asynchronous Services

Request handlers in ASP.NET Core applications are asynchronous. To enable asynchronous services, we called the **UseAsyncEngine** method at application startup:

```csharp
services.ConfigureReportingServices(configurator => { 
    configurator.ConfigureReportDesigner(designerConfigurator => { 
        //.. 
    }); 
    configurator.ConfigureWebDocumentViewer(viewerConfigurator => { 
        //.. 
    }); 
    configurator.UseAsyncEngine(); 
});
``` 
 
## Notes 

You can use an asynchronous engine in the following scenarios: 
- Handle the End User Report Designer and Web Document Viewer events to load data or create a document asynchronously. The key point is that these actions can be performed in the context of the current HTTP request in the [WebDocumentViewerOperationLogger](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Web.WebDocumentViewer.WebDocumentViewerOperationLogger) and  [PreviewReportCustomizationService](https://docs.devexpress.com/XtraReports/DevExpress.XtraReports.Web.ReportDesigner.Services.PreviewReportCustomizationService) class methods. 
- Drill Through navigation in the Web Document Viewer. Use the  **IDrillThroughProcessorAsync** interface. 
- If an application uses JWT-based Authentication, use the **IWebDocumentViewerExportResultUriGeneratorAsync** interface to load exported documents to the storage asynchronously. 
 
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-asp-net-core-use-async-engine&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-asp-net-core-use-async-engine&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->

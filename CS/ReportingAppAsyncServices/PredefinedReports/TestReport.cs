using System;
using System.Drawing;
using DevExpress.XtraReports.UI;

namespace ReportingAppAsyncServices.PredefinedReports
{
    public partial class TestReport
    {
        public TestReport()
        {
            InitializeComponent();
        }

        private void tableCell4_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as XRTableCell).Text += " Customized";
        }
    }
}

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Services;
using TKProcessor.Services.Reports;
using TKProcessor.WPF.Models;
using TKProcessor.WPF.Views;

namespace TKProcessor.WPF.ViewModels
{
    public class ReportViewModel : Screen
    {
        public void LoadReport(Microsoft.Reporting.WinForms.ReportViewer reportViewer, string reportName, DataTable dt)
        {
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource
            {
                Name = "dsData", // Name of the DataSet we set in .rdlc
                Value = dt
            };

            reportViewer.LocalReport.ReportPath = $"reports\\{reportName}.rdl";

            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            reportViewer.RefreshReport();
        }
    }

    public static class ReportDataFactory
    {
        public static DataTable GetData(string reportName)
        {
            DataTable result = null;

            if (reportName == "TK_AbsencesRpt")
            {
                result = DataTableHelpers.ToDataTable(new AbsenceReportService().GetData());
            }

            return result;
        }
    }
}

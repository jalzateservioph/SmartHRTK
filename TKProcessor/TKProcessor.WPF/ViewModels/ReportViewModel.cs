using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Services;
using TKProcessor.WPF.Models;
using TKProcessor.WPF.Views;

namespace TKProcessor.WPF.ViewModels
{
    public class ReportViewModel : Conductor<Report>.Collection.OneActive
    {
        public ReportViewModel()
        {
            Items.Add(new Report() { DisplayName = "Absence Report", Name = "TK_AbsencesRpt" });
        }

        public void SelectionChanged()
        {
            if (ActiveItem != null)
            {
                var reportView = (Views.First().Value as ReportView);

                reportView.LoadReport(ActiveItem.Name, ReportDataFactory.GetData(ActiveItem.Name));
            }
        }
    }

    public class ReportDataFactory
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

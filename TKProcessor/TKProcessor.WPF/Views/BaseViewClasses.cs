using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Views
{
    public interface IReportView
    {
        ReportViewer ReportViewer { get; }
    }
}
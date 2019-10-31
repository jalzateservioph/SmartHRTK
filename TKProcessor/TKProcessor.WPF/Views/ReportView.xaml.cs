using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TKProcessor.WPF.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
        }

        public void LoadReport(string reportName, DataTable dt)
        {
            ReportDataSource reportDataSource = new ReportDataSource
            {
                Name = "dsData", // Name of the DataSet we set in .rdlc
                Value = dt
            };

            reportViewer.LocalReport.ReportPath = $"reports\\{reportName}.rdl"; // Path of the rdlc file



            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            reportViewer.RefreshReport();
        }
    }
}

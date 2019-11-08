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
    /// Interaction logic for ReportAbsenceView.xaml
    /// </summary>
    public partial class ReportAbsenceView : UserControl, IReportView
    {
        public ReportAbsenceView()
        {
            InitializeComponent();
        }

        public ReportViewer ReportViewer
        {
            get => reportViewer;
        }
    }
}

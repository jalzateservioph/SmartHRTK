using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class DashboardViewModel : Conductor<Dashboard>
    {
        readonly EmployeeService employeeService;
        readonly DailyTransactionRecordService dtrService;
        private DateTime startDate;
        private DateTime endDate;

        public DashboardViewModel()
        {
            employeeService = new EmployeeService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            dtrService = new DailyTransactionRecordService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            StartDate = DateTime.Today.AddDays(-DateTime.Today.Day).AddDays(1);

            EndDate = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day);

            PropertyChanged += DashboardViewModel_PropertyChanged;

            Populate();
        }

        private void DashboardViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(StartDate) || e.PropertyName == nameof(EndDate)))
            {
                if (StartDate > EndDate)
                    EndDate = StartDate;
            }
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                EmployeeCount = employeeService.List().Where(emp => !emp.TerminationDate.HasValue || (emp.TerminationDate.HasValue && emp.TerminationDate.Value.Date > startDate.Date)).Count();
                //ActiveEmployeeCount = employeeService.List().Where(i => i.TerminationDate == null).Count();
                ActualHoursWorked = dtrService.List().Where(dtr => dtr.TransactionDate >= startDate && dtr.TransactionDate <= endDate).Sum(dtr => dtr.WorkHours);
                RegularHoursWorked = dtrService.List().Where(dtr => dtr.TransactionDate >= startDate && dtr.TransactionDate <= endDate).Sum(dtr => dtr.RegularWorkHours);
                AbsentHoursCount = dtrService.List().Where(dtr => dtr.TransactionDate >= startDate && dtr.TransactionDate <= endDate).Sum(dtr => dtr.AbsentHours);

                App.Current.Dispatcher.Invoke(() => NotifyOfPropertyChange(""));
            });
        }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                NotifyOfPropertyChange();
            }
        }

        public int EmployeeCount { get; set; }

        public int ActiveEmployeeCount { get; set; }

        public decimal ActualHoursWorked { get; set; }

        public decimal RegularHoursWorked { get; set; }

        public decimal AbsentHoursCount { get; set; }
    }
}

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

            StartDate = DateTime.Today.AddDays(-DateTime.Today.Day);

            EndDate = DateTime.Today.AddDays(-DateTime.Today.Day).AddMonths(1).AddDays(-1);

            PropertyChanged += DashboardViewModel_PropertyChanged;
        }

        private void DashboardViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if((e.PropertyName == nameof(StartDate) || e.PropertyName == nameof(EndDate)) &&
                StartDate > EndDate)
            {
                EndDate = StartDate;
            }
        }

        public void Populate()
        {
            NotifyOfPropertyChange(() => EmployeeCount);
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

        public int EmployeeCount { get => employeeService.List().Count(); }

        public int ActiveEmployeeCount { get => employeeService.List().Where(i => i.TerminationDate == null).Count(); }

        public decimal HoursWorked { get => dtrService.List().Sum(i => i.RegularWorkHours); }

        public int AbsentEmployeeCount { get; }

        public decimal AbsentHoursCount { get; }
    }
}

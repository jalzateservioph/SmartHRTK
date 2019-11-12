using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Services.Maintenance;
using TKProcessor.Services.Reports;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Models;
using TKProcessor.WPF.Views;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class ReportAbsenceViewModel : ReportViewModel
    {
        private BindingList<Employee> employees;
        private BindingList<Shift> shifts;

        private BindingList<Employee> employeeList;
        private BindingList<Shift> shiftList;

        private DateTime startDate;
        private DateTime endDate;

        readonly IMapper mapper;

        public ReportAbsenceViewModel()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TK.Employee, Employee>();
                cfg.CreateMap<Employee, TK.Employee>();

                cfg.CreateMap<TK.Shift, Shift>();

                cfg.CreateMap<TK.User, User>();
            }).CreateMapper();

            Initialize();
        }

        private void Initialize()
        {
            EmployeeList = new BindingList<Employee>();
            Employees = new BindingList<Employee>();

            ShiftList = new BindingList<Shift>();
            Shifts = new BindingList<Shift>();

            using (var empService = new EmployeeService(Session.Default.CurrentUser.Id))
            {
                foreach (var emp in empService.List())
                {
                    EmployeeList.Add(mapper.Map<Employee>(emp));
                }
            }

            using (var shiftService = new ShiftService(Session.Default.CurrentUser.Id))
            {
                foreach (var shift in shiftService.List())
                {
                    ShiftList.Add(mapper.Map<Shift>(shift));
                }
            }

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        public void Generate()
        {
            AbsenceReportService service = new AbsenceReportService()
            {
                Employees = Employees.Select(i => mapper.Map<TK.Employee>(i)),
                Shifts = Shifts.Select(i => mapper.Map<TK.Shift>(i)),
                StartDate = StartDate,
                EndDate = EndDate
            };

            var data = service.GetData();

            IReportView view = Views.FirstOrDefault().Value as IReportView;

            LoadReport(view.ReportViewer, "TK_AbsencesRpt", data.ToDataTable());
        }

        public BindingList<Employee> EmployeeList
        {
            get => employeeList;
            set
            {
                employeeList = value;
                NotifyOfPropertyChange();
            }
        }
        public BindingList<Shift> ShiftList
        {
            get => shiftList;
            set
            {
                shiftList = value;
                NotifyOfPropertyChange();
            }
        }

        public BindingList<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                NotifyOfPropertyChange();
            }
        }
        public BindingList<Shift> Shifts
        {
            get => shifts;
            set
            {
                shifts = value;
                NotifyOfPropertyChange();
            }
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
    }
}

using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services.Reports;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class ReportAbsenceViewModel : ReportViewModel
    {
        private IList<Employee> employees;
        private IList<Shift> shifts;
        private DateTime startDate;
        private DateTime endDate;

        IMapper mapper;
        private BindingList<Employee> employeeList;
        private BindingList<Shift> shiftList;

        public ReportAbsenceViewModel()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, TK.Employee>();
                cfg.CreateMap<Shift, TK.Shift>();
            }).CreateMapper();
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

        public IList<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                NotifyOfPropertyChange();
            }
        }
        public IList<Shift> Shifts
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

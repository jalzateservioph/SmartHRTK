using AutoMapper;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TKProcessor.Common;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class DailyTransactionRecordViewModel : ViewModelBase<DailyTransactionRecord>
    {
        readonly IMapper mapper;
        readonly DailyTransactionRecordService dtrService;
        readonly EmployeeService employeeService;
        readonly JobGradeBandService payCodeService;
        readonly SaveFileDialog saveDiag;

        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _payOutDate;

        private IList<string> _selectedPayrollCodes;
        private IList<string> _payrollCodeList;
        private IList<Employee> employeesList;
        private IList<Employee> selectedEmployees;

        public DailyTransactionRecordViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            dtrService = new DailyTransactionRecordService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            employeeService = new EmployeeService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            payCodeService = new JobGradeBandService();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyTransactionRecord, TK.DailyTransactionRecord>();
                cfg.CreateMap<TK.DailyTransactionRecord, DailyTransactionRecord>()
                   .AfterMap((tkdtr, wpfdtr) => { wpfdtr.IsDirty = false; });

                cfg.CreateMap<Employee, TK.Employee>();
                cfg.CreateMap<TK.Employee, Employee>();

                cfg.CreateMap<Shift, TK.Shift>();
                cfg.CreateMap<TK.Shift, Shift>();

                cfg.CreateMap<User, TK.User>();
                cfg.CreateMap<TK.User, User>();
            }).CreateMapper();

            saveDiag = new SaveFileDialog()
            {
                Filter = "Excel File (*.xlsx)|*.xlsx"
            };

            PropertyChanged += DailyTransactionRecordViewModel_PropertyChanged;

            InitializeFilters();

            AutoFilter = false;
        }

        private void InitializeFilters()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            PayOutDate = DateTime.Now;

            Task.Run(() =>
            {
                StartProcessing();

                RaiseMessage("Loading filters...");

                App.Current.Dispatcher.Invoke(() =>
                {
                    PayrollCodeList = payCodeService.List();

                    EmployeeList = employeeService.List().Select(i => mapper.Map<Employee>(i)).ToList();
                });

                RaiseMessage("Filters loaded");

                EndProcessing();
            });
        }

        private void DailyTransactionRecordViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(StartDate) || e.PropertyName == nameof(EndDate)) &&
                StartDate > EndDate)
            {
                EndDate = StartDate;
            }
        }

        public void PopulateAsync()
        {
            Task.Run(() =>
            {
                StartProcessing();

                Populate();

                EndProcessing();
            });
        }

        public void Populate()
        {
            try
            {
                Items.Clear();

                var retrievedItems = dtrService.List(StartDate, EndDate, SelectedPayrollCodes, SelectedEmployees.Select(i => mapper.Map<TK.Employee>(i)).ToList());

                foreach (TK.DailyTransactionRecord item in retrievedItems)
                {
                    Items.Add(mapper.Map<DailyTransactionRecord>(item));
                }
            }
            catch (Exception ex)
            {
                eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
            }
        }

        public void Save()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Updating DTR records...", MessageType.Information, 0));

                    foreach (var item in View.Cast<DailyTransactionRecord>().Where(i => i.IsDirty))
                    {
                        dtrService.Save(mapper.Map<TK.DailyTransactionRecord>(item));
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Updated DTR records", MessageType.Information));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void Process()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Processing DTR records...", MessageType.Information, 0));

                    dtrService.Process(
                        StartDate, 
                        EndDate, 
                        SelectedPayrollCodes, 
                        SelectedEmployees.Select(i => mapper.Map<TK.Employee>(i)).ToList(), 
                        message => RaiseMessage(message)
                    );

                    Populate();

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Processing complete", MessageType.Information));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void ExportToDP()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Export to dynamic pay has been started.", MessageType.Information, 0));

                    Populate();

                    dtrService.ExportToDP(
                        StartDate, 
                        EndDate, 
                        PayOutDate, 
                        SelectedPayrollCodes, 
                        SelectedEmployees.Select(i => mapper.Map<TK.Employee>(i)).ToList(), 
                        message => RaiseMessage(message)
                    );

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Export to dynamic pay complete.", MessageType.Success));

                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void ExportToExcel()
        {
            if (saveDiag.ShowDialog() == true)
            {
                Task.Run(() =>
                {
                    StartProcessing();

                    try
                    {
                        Populate();

                        var data = dtrService.GetExportExcelData(StartDate, EndDate, SelectedPayrollCodes, SelectedEmployees.Select(i => mapper.Map<TK.Employee>(i)));
                      
                        ExcelFileHandler.Export(saveDiag.FileName, data);

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Exported to {saveDiag.FileName}", MessageType.Success));
                    }
                    catch (Exception ex)
                    {
                        eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                    }

                    EndProcessing();
                });
            }
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription(nameof(Employee.EmployeeCode), ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription(nameof(DailyTransactionRecord.TransactionDate), ListSortDirection.Descending));
        }

        public override bool Filter(object o)
        {
            var entity = o as DailyTransactionRecord;
            var splitValue = FilterString.Split(',');

            if (splitValue.Any(str => entity.Employee.EmployeeCode.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.TransactionDate.Value.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.TransactionDate.Value.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                NotifyOfPropertyChange();
            }
        }

        public DateTime PayOutDate
        {
            get => _payOutDate;
            set
            {
                _payOutDate = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<string> PayrollCodeList
        {
            get => _payrollCodeList ?? (_payrollCodeList = new List<string>());
            set
            {
                _payrollCodeList = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<string> SelectedPayrollCodes
        {
            get => _selectedPayrollCodes ?? (_selectedPayrollCodes = new List<string>());
            set
            {
                _selectedPayrollCodes = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<Employee> EmployeeList
        {
            get => employeesList ?? (employeesList = new List<Employee>());
            set
            {
                employeesList = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<Employee> SelectedEmployees
        {
            get => selectedEmployees ?? (SelectedEmployees = new List<Employee>());
            set
            {
                selectedEmployees = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

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
using System.Windows.Input;
using TKProcessor.Common;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class DailyTransactionRecordViewModel : EditableViewModelBase<DailyTransactionRecord>
    {
        readonly IMapper mapper;
        readonly DailyTransactionRecordService dtrService;
        readonly EmployeeService employeeService;
        readonly JobGradeBandService payCodeService;
        readonly SaveFileDialog saveDiag;

        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _payOutDate;

        private ObservableCollection<string> _selectedPayrollCodes;
        private ObservableCollection<string> _payrollCodeList;

        private ICollectionView employeeListView;
        private ObservableCollection<Employee> employeesList;
        private ICollectionView selectedEmployeesView;
        private ObservableCollection<Employee> selectedEmployees;

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

                cfg.CreateMap<TK.EmployeeWorkSite, EmployeeWorkSite>();
                cfg.CreateMap<EmployeeWorkSite, TK.EmployeeWorkSite>();

                cfg.CreateMap<WorkSite, TK.WorkSite>();
                cfg.CreateMap<TK.WorkSite, WorkSite>();

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
                    PayrollCodeList = new ObservableCollection<string>(employeeService.List().Select(i => i.JobGradeBand).Distinct().OrderBy(i => i));

                    SelectedPayrollCodes = new ObservableCollection<string>();

                    SelectedPayrollCodes.CollectionChanged += (o, e) =>
                    {
                        NotifyOfPropertyChange(() => SelectedPayrollCodes);
                    };

                    EmployeeList = new ObservableCollection<Employee>(employeeService.List().Select(i => mapper.Map<Employee>(i)).OrderBy(i => i.EmployeeCode));

                    EmployeeListView = CollectionViewSource.GetDefaultView(EmployeeList);

                    EmployeeListView.SortDescriptions.Add(new SortDescription("EmployeeCode", ListSortDirection.Ascending));

                    SelectedEmployees = new ObservableCollection<Employee>();

                    SelectedEmployees.CollectionChanged += (o, e) =>
                    {
                        NotifyOfPropertyChange(() => SelectedEmployees);
                    };

                    SelectedEmployeesView = CollectionViewSource.GetDefaultView(SelectedEmployees);

                    SelectedEmployeesView.SortDescriptions.Add(new SortDescription("EmployeeCode", ListSortDirection.Ascending));

                    EmployeeListView.Filter += (o) =>
                    {
                        Employee emp = (Employee)o;

                        if (SelectedEmployees.Cast<Employee>().Any(i => i.Id == emp.Id))
                            return false;

                        if (string.IsNullOrEmpty(EmployeeListViewSearch))
                            return true;

                        if (emp.EmployeeCode.ToLower().Contains(EmployeeListViewSearch.ToLower()))
                            return true;

                        if (emp.FullName.ToLower().Contains(EmployeeListViewSearch.ToLower()))
                            return true;

                        return false;
                    };

                    SelectedEmployeesView.Filter += (o) =>
                    {
                        Employee emp = (Employee)o;

                        if (EmployeeListView.Cast<Employee>().Any(i => i.Id == emp.Id))
                            return false;

                        if (string.IsNullOrEmpty(SelectedEmployeesViewSearch))
                            return true;

                        if (emp.EmployeeCode.ToLower().Contains(SelectedEmployeesViewSearch.ToLower()))
                            return true;

                        if (emp.FullName.ToLower().Contains(SelectedEmployeesViewSearch.ToLower()))
                            return true;

                        return false;
                    };

                    SelectedEmployeesView.Refresh();
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
                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Loading DTR records...", MessageType.Information, 0));

                Items.Clear();

                var retrievedItems = dtrService.List(
                    StartDate, EndDate, (SelectedPayrollCodes.Count == 0 ? PayrollCodeList : SelectedPayrollCodes),
                    (SelectedEmployees.Count == 0 ? EmployeeList : SelectedEmployees).Select(i => mapper.Map<TK.Employee>(i)).ToList()
                );

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
                        (SelectedPayrollCodes.Count == 0 ? PayrollCodeList : SelectedPayrollCodes),
                        (SelectedEmployees.Count == 0 ? EmployeeList : SelectedEmployees).Select(i => mapper.Map<TK.Employee>(i)).ToList(),
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
                        (SelectedPayrollCodes.Count == 0 ? PayrollCodeList : SelectedPayrollCodes),
                        (SelectedEmployees.Count == 0 ? EmployeeList : SelectedEmployees).Select(i => mapper.Map<TK.Employee>(i)).ToList(),
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

                        var data = dtrService.GetExportExcelData(
                            StartDate, EndDate, (SelectedPayrollCodes.Count == 0 ? PayrollCodeList : SelectedPayrollCodes),
                            (SelectedEmployees.Count == 0 ? EmployeeList : SelectedEmployees).Select(i => mapper.Map<TK.Employee>(i))
                        );

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

        public void OpenEmployeeSelector()
        {
            StartEditing();
        }

        public void AddSelectedEmployees(Employee source)
        {
            SelectedEmployees.Add(source);

            FilterEmployeeFilter();
        }

        public void RemoveSelectedEmployees(Employee source)
        {
            SelectedEmployees.Remove(source);

            FilterEmployeeFilter();
        }

        public void FilterEmployeeFilter()
        {
            try
            {
                EmployeeListView.Refresh();

                SelectedEmployeesView.Refresh();
            }
            catch
            {

            }
        }

        public void InvokeEmployeeFilter(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            FilterEmployeeFilter();
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription("Employee.EmployeeCode", ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription("TransactionDate", ListSortDirection.Descending));
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

        public ObservableCollection<string> PayrollCodeList
        {
            get => _payrollCodeList;
            set
            {
                _payrollCodeList = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<string> SelectedPayrollCodes
        {
            get => _selectedPayrollCodes;
            set
            {
                _selectedPayrollCodes = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get => employeesList;
            set
            {
                employeesList = value;
                NotifyOfPropertyChange();
            }
        }

        public string EmployeeListViewSearch
        {
            get;
            set;
        }

        public ICollectionView EmployeeListView
        {
            get => employeeListView;
            set
            {
                employeeListView = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<Employee> SelectedEmployees
        {
            get => selectedEmployees;
            set
            {
                selectedEmployees = value;
                NotifyOfPropertyChange();
            }
        }

        public string SelectedEmployeesViewSearch
        {
            get;
            set;
        }

        public ICollectionView SelectedEmployeesView
        {
            get => selectedEmployeesView;
            set
            {
                selectedEmployeesView = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
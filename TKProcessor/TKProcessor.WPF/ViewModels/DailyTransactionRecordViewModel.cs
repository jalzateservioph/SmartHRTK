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
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class DailyTransactionRecordViewModel : ViewModelBase<DailyTransactionRecord>
    {
        readonly IMapper mapper;
        readonly DailyTransactionRecordService service;
        readonly JobGradeBandService payCodeService;
        readonly SaveFileDialog saveDiag;

        private ObservableCollection<string> _payrollCodeList;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _payOutDate;

        public DailyTransactionRecordViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new DailyTransactionRecordService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            payCodeService = new JobGradeBandService();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyTransactionRecord, TK.DailyTransactionRecord>();
                cfg.CreateMap<TK.DailyTransactionRecord, DailyTransactionRecord>();

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

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            PayOutDate = DateTime.Now;

            PropertyChanged += DailyTransactionRecordViewModel_PropertyChanged;

            Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    PayrollCodeList = new ObservableCollection<string>(payCodeService.List());
                    PayrollCode = PayrollCodeList[0];
                });
            });

            AutoFilter = false;
        }

        private void DailyTransactionRecordViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(StartDate) || e.PropertyName == nameof(EndDate)) &&
                StartDate > EndDate)
            {
                EndDate = StartDate;
            }
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    PayrollCodeList = new ObservableCollection<string>(payCodeService.List());

                    PayrollCode = PayrollCodeList[0];

                    Items.Clear();

                    foreach (TK.DailyTransactionRecord item in service.List(StartDate, EndDate, PayrollCode))
                    {
                        Items.Add(mapper.Map<DailyTransactionRecord>(item));
                    }
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void Save()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Updating DTR records...", MessageType.Information));

                    foreach (var item in Items)
                    {
                        service.Save(mapper.Map<TK.DailyTransactionRecord>(item));
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
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Processing DTR records...", MessageType.Information));

                    service.Process(StartDate, EndDate, PayrollCode);

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

        public void Export()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Export to dynamic pay has been started.", MessageType.Information));

                    service.Export(StartDate, EndDate, PayOutDate, PayrollCode);

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

                    var DTRs = service.List(StartDate, EndDate, PayrollCode);

                    DataTableHelpers.ToDataTable(DTRs);

                    EndProcessing();
                });
            }
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
        public string PayrollCode { get; set; }
        public ObservableCollection<string> PayrollCodeList
        {
            get => _payrollCodeList;
            set
            {
                _payrollCodeList = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

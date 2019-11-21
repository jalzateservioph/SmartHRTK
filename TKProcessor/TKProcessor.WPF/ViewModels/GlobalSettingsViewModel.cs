using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKModels = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class GlobalSettingsViewModel : ViewModelBase<GlobalSettings>, IDisposable
    {
        private readonly IMapper mapper;
        private readonly GlobalSettingsService service;
        private readonly PayrollCodeService payCodeService;
        private readonly PayPackageService payPackageService;
        private readonly JobGradeBandService jobGradeBandService;
        private ObservableCollection<string> _payrollCodeList;
        private ObservableCollection<string> _payPackageList;

        public GlobalSettingsViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GlobalSettings, TKModels.GlobalSetting>();
                cfg.CreateMap<TKModels.GlobalSetting, GlobalSettings>();

                cfg.CreateMap<Mapping, TKModels.Mapping>()
                    .AfterMap((from, to) => { to.Source = to.Source ?? ""; });
                cfg.CreateMap<TKModels.Mapping, Mapping>()
                    .AfterMap((from, to) => { to.Source = to.Source ?? ""; });

                cfg.CreateMap<SelectionSetting, TKModels.SelectionSetting>();
                cfg.CreateMap<TKModels.SelectionSetting, SelectionSetting>();

                cfg.CreateMap<User, TKModels.User>();
                cfg.CreateMap<TKModels.User, User>();
            }).CreateMapper();

            service = new GlobalSettingsService();

            payCodeService = new PayrollCodeService();

            payPackageService = new PayPackageService();

            jobGradeBandService = new JobGradeBandService();

            Populate();
        }

        public void Populate()
        {
            Task.Run(() =>
            {

                StartProcessing();

                try
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        // load payroll codes
                        PayrollCodeList = new ObservableCollection<string> { "" };

                        foreach (var item in payCodeService.List().OrderBy(i => i.Code).Select(i => i.Code).ToList())
                        {
                            PayrollCodeList.Add(item);
                        }

                        // load pay packages
                        PayPackageList = new ObservableCollection<string>() { "" };

                        foreach (var item in payPackageService.List().Select(i => i.Code))
                        {
                            PayPackageList.Add(item);
                        }

                    });

                    // load global settings
                    TKModels.GlobalSetting data = service.List().ToList().FirstOrDefault();

                    if (data != default(TKModels.GlobalSetting))
                        ActivateItem(mapper.Map<GlobalSettings>(data));
                    else
                        ActivateItem(new GlobalSettings());

                    int index = 0;

                    // load pay packages
                    var list = typeof(DailyTransactionRecord).GetProperties();
                    var props = list.Where(i => (i.PropertyType == typeof(decimal) || i.PropertyType == typeof(decimal?)));

                    foreach (var propInfo in props)
                    {
                        if (propInfo.Name.Contains("Actual"))
                            continue;

                        if (ActiveItem.PayrollCodeMappings.Any(i => i.Target == propInfo.Name))
                            continue;

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActiveItem.PayrollCodeMappings.Add(new Mapping() { Target = propInfo.Name, Source = "", Order = index++ });
                        });
                    }

                    App.Current.Dispatcher.Invoke(() => { ActiveItem.ViewAndSort(); });

                    index = 0;

                    // load pay packages
                    foreach (var jobGradeBand in jobGradeBandService.List())
                    {
                        if (ActiveItem.PayPackageMappings.Any(i => i.Target == jobGradeBand))
                            continue;

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActiveItem.PayPackageMappings.Add(new Mapping() { Target = jobGradeBand, Source = "", Order = index++ });
                        });
                    }

                    App.Current.Dispatcher.Invoke(() => { ActiveItem.ViewAndSort(); });

                    index = 0;

                    // load auto approve list
                    foreach (var propInfo in typeof(TKModels.DailyTransactionRecord).GetProperties().Where(i => i.PropertyType == typeof(decimal) && i.Name.Contains("Approved")))
                    {
                        string name = propInfo.Name.Replace("Approved", "");

                        if (ActiveItem.AutoApproveDTRFieldsList.Any(i => i.Name == name))
                            continue;

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ActiveItem.AutoApproveDTRFieldsList.Add(new SelectionSetting() { Name = name, IsSelected = true, DisplayOrder = index++ });
                        });
                    }

                    App.Current.Dispatcher.Invoke(() => { ActiveItem.ViewAndSort(); });

                    eventAggregator.BeginPublishOnUIThread(new NewMessageEvent($"Retrieved global settings", MessageType.Information));
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
            try
            {
                service.Save(mapper.Map<TKModels.GlobalSetting>(ActiveItem));

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Global settings saved", MessageType.Success));
            }
            catch (Exception ex)
            {
                eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
            }
        }

        public void Dispose()
        {
            service.Dispose();

            payCodeService.Dispose();

            payPackageService.Dispose();

            jobGradeBandService.Dispose();
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
        public ObservableCollection<string> PayPackageList
        {
            get => _payPackageList;
            set
            {
                _payPackageList = value;
                NotifyOfPropertyChange();
            }
        }

    }
}

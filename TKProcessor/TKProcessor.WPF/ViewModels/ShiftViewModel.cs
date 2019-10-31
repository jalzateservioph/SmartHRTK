using AutoMapper;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKModels = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class ShiftViewModel : ViewModelBase<Shift>
    {
        readonly IEventAggregator eventAggregator;
        readonly IWindowManager windowManager;
        readonly IMapper mapper;
        readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        readonly ShiftService service;
        private Shift _currentItem;

        public ShiftViewModel()
        {
            service = new ShiftService(Session.Default.CurrentUser?.Id ?? Guid.Empty) { AutoSaveChanges = true };

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Shift, TKModels.Shift>();
                cfg.CreateMap<TKModels.Shift, Shift>()
                    .AfterMap((model, appmodel) => { appmodel.IsDirty = false; });

                cfg.CreateMap<User, TKModels.User>();
                cfg.CreateMap<TKModels.User, User>();

                cfg.CreateMap<Shift, Shift>();
                cfg.CreateMap<Shift, Shift>();

            }).CreateMapper();

            openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx"
            };
            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx"
            };

            PropertyChanged += ShiftViewModel_PropertyChanged;

            // change this in the future 

            FlexiTypes = new Dictionary<string, int> { { "", -1 } };

            System.Collections.IList list = Enum.GetValues(typeof(TKModels.FlextimeType));

            for (int i = 0; i < list.Count; i++)
            {
                TKModels.FlextimeType item = (TKModels.FlextimeType)list[i];
                FlexiTypes.Add(item.ToString(), i);
            }

            FlexiIncrements = new Dictionary<string, int> { { "", -1 } };

            list = Enum.GetValues(typeof(TKModels.Increment));

            for (int i = 0; i < list.Count; i++)
            {
                TKModels.Increment item = (TKModels.Increment)list[i];
                FlexiIncrements.Add(item.ToString(), i);
            }

            // change this in the future 

            New();

            Populate();
        }

        private void ShiftViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ActiveItem))
                CurrentItem = mapper.Map<Shift>(ActiveItem);
        }

        public ShiftViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : this()
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    Items.Clear();

                    foreach (var item in service.List().Where(i => i.IsActive).Select(i => mapper.Map<Shift>(i)))
                    {
                        Items.Add(item);
                    }

                    New();

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} shifts ", MessageType.Success));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public bool CanSave { get => CurrentItem.IsValid; }

        public void Save()
        {
            StartProcessing();

            try
            {

                if(string.IsNullOrEmpty(CurrentItem.ShiftCode))
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Cannot save shift without shift code", MessageType.Success));

                if (CurrentItem.ScheduleIn == null)
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Cannot save shift without schedule in", MessageType.Success));

                if (CurrentItem.ScheduleOut == null)
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Cannot save shift without schedule out", MessageType.Success));

                if (CurrentItem.RequiredWorkHours == 0)
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Cannot save shift without required work hours", MessageType.Success));

                service.Save(mapper.Map<TKModels.Shift>(CurrentItem));

                if (Items.Any(i => i.Id == CurrentItem.Id))
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Updated shift {CurrentItem.ShiftCode}", MessageType.Success));
                }
                else
                {
                    ActivateItem(CurrentItem);
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Added new shift {CurrentItem.ShiftCode}", MessageType.Success));
                }

            }
            catch (Exception ex)
            {
                eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
            }

            EndProcessing();
        }

        public void New()
        {
            StartProcessing();

            using (var globalSettingsService = new GlobalSettingsService())
            {
                var a = globalSettingsService.List().Select(i => new { i.DefaultNDStart, i.DefaultNDEnd }).FirstOrDefault();

                if (a != null)
                {
                    CurrentItem = new Shift()
                    {
                        IsDirty = false,
                        NightDiffStart = a.DefaultNDStart,
                        NightDiffEnd = a.DefaultNDEnd
                    };
                }
            }

            EndProcessing();
        }

        public void Import()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    bool? result = null;

                    App.Current.Dispatcher.Invoke(() => { result = openFileDialog.ShowDialog(); });

                    if (result == true)
                    {
                        service.Import(openFileDialog.FileName, null);

                        Populate();

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Successfully imported {Path.GetFileName(openFileDialog.FileName)}", MessageType.Success));
                    }

                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void DownloadTemplate()
        {
            if (saveFileDialog.ShowDialog() == true)
            {
                service.ExportTemplate(saveFileDialog.FileName);

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saved file to {saveFileDialog.FileName}", MessageType.Success));
            }
        }

        public Shift CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                NotifyOfPropertyChange();
            }
        }

        public Dictionary<string, int> FlexiTypes { get; }
        public Dictionary<string, int> FlexiIncrements { get; }
    }
}

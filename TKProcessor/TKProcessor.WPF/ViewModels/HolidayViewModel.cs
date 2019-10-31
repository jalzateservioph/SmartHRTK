using AutoMapper;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TKProcessor.Services;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class HolidayViewModel : EditableViewModelBase<Holiday>, IDisposable
    {
        private readonly IMapper mapper;
        private readonly HolidayService service;
        private readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        private HolidaySummary holidaySummary;
        private bool _isCheckedAll;

        public HolidayViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            PropertyChanged += HolidayViewModel_PropertyChanged;

            service = new HolidayService();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Holiday, TK.Holiday>();
                cfg.CreateMap<TK.Holiday, Holiday>()
                    .AfterMap((model, wpfmodel) => { wpfmodel.IsDirty = false; });

                cfg.CreateMap<User, TK.User>();
                cfg.CreateMap<TK.User, User>();
            }).CreateMapper();

            openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx"
            };
            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx"
            };

            Populate();

            AutoFilter = true;
        }

        private void HolidayViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsCheckedAll))
                Items.ToList().ForEach(i => i.IsSelected = IsCheckedAll);
        }

        public void New()
        {
            holidaySummary = new HolidaySummary();

            StartEditing();
        }

        public void Save()
        {
            foreach (var item in Items.Where(i => i.IsDirty))
            {
                service.Save(mapper.Map<TK.Holiday>(item));
            }
        }

        public void SaveRecords()
        {
            eventAggregator.BeginPublishOnUIThread(new NewMessageEvent($"Saving edited records... ", MessageType.Information));

            foreach (var item in Items.Where(i => i.IsDirty))
            {
                service.Save(mapper.Map<TK.Holiday>(item));
            }

            eventAggregator.BeginPublishOnUIThread(new NewMessageEvent($"Records saved", MessageType.Success));
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    Items.Clear();

                    foreach (var item in service.List().Where(i => i.IsActive))
                        Items.Add(mapper.Map<Holiday>(item));

                    eventAggregator.BeginPublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} holiday entries ", MessageType.Success));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void Delete()
        {
            StartProcessing();

            Task.Run(() =>
            {
                try
                {

                    var forDelete = Items.Where(i => i.IsSelected).ToList();

                    foreach (var item in forDelete)
                        service.DeleteHard(mapper.Map<TK.Holiday>(item));

                    Populate();

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleted {forDelete.Count} holidays", MessageType.Success));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }
            });

            EndProcessing();
        }

        public void Import()
        {
            Task.Run(() => {
                StartProcessing();

            try
            {
                bool? result = null;

                App.Current.Dispatcher.Invoke(() => { result = openFileDialog.ShowDialog(); });

                if (result == true)
                {
                    service.Import(openFileDialog.FileName, null);

                    Populate();

                    eventAggregator.BeginPublishOnUIThread(
                        new NewMessageEvent(
                            $"Successfully imported {Path.GetFileName(openFileDialog.FileName)}",
                            MessageType.Success
                        )
                    );
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

        public override bool Filter(object o)
        {
            if (string.IsNullOrEmpty(FilterString))
                return true;

            var entity = (Holiday)o;
            var splitValue = FilterString.Split(',');

            if (splitValue.Any(str => entity.Name.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => ((TK.HolidayType)entity.Type).ToString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.Date.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.Date.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
        }

        public void Dispose()
        {
            service.Dispose();
        }

        public HolidaySummary HolidaySummary
        {
            get => holidaySummary;
            set
            {
                holidaySummary = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsCheckedAll
        {
            get => _isCheckedAll;
            set
            {
                _isCheckedAll = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

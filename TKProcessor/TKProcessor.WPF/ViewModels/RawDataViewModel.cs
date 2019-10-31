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
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class RawDataViewModel : ViewModelBase<RawData>, IDisposable
    {
        readonly IMapper mapper;
        readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        readonly RawDataService service;
        private bool _isCheckedAll;

        public RawDataViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            PropertyChanged += RawDataViewModel_PropertyChanged;

            service = new RawDataService();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RawData, TK.RawData>();
                cfg.CreateMap<TK.RawData, RawData>();

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
        }

        private void RawDataViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsCheckedAll))
                Items.ToList().ForEach(i => i.IsSelected = IsCheckedAll);
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieving raw data from database...", MessageType.Success));

                    Items.Clear();

                    foreach (var item in service.List().Where(i => i.IsActive))
                    {
                        Items.Add(mapper.Map<RawData>(item));
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} records", MessageType.Success));
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
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    int count = 0;

                    foreach (var item in Items.Where(i => i.IsSelected))
                    {
                        service.DeleteHard(mapper.Map<TK.RawData>(item));
                        count++;
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleted {count} records", MessageType.Success));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
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

        public override bool Filter(object o)
        {
            if (string.IsNullOrEmpty(FilterString))
                return true;

            var entity = (RawData)o;
            var splitValue = FilterString.Trim(',').Split(',');

            if (splitValue.Any(str => entity.BiometricsId.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.TransactionDateTime.Value.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.TransactionDateTime.Value.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }

        public void Dispose()
        {
            service.Dispose();
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

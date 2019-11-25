using AutoMapper;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class RawDataViewModel : EditableViewModelBase<RawData>, IDisposable
    {
        readonly IMapper mapper;
        readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        readonly RawDataService service;

        private string importFile;
        private string[] targetValues;
        private ObservableCollection<ExcelMappingModel> importMapping;
        private ObservableCollection<string> sourceValues;
        private DataTable inputData;
        private ObservableCollection<RawData> outputData;

        public RawDataViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            PropertyChanged += RawDataViewModel_PropertyChanged;

            service = new RawDataService(Session.Default.CurrentUser.Id);

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

            targetValues = new string[] {
                "Biometrics Id",
                "Transaction Type",
                "Transaction Date/Time",
                "Schedule Date"
            };
        }

        private void RawDataViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsCheckedAll))
                View.Cast<RawData>().ToList().ForEach(i => i.IsSelected = IsCheckedAll);
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

        public void OpenCustomImport()
        {
            StartEditing();

            ImportCustomFile = "";
            InputData = null;
            outputData = null;

            ImportMapping.Clear();

            foreach (var item in targetValues)
            {
                ImportMapping.Add(new ExcelMappingModel() { Target = item, Source = "" });
            }
        }

        public void ChooseCustomFile()
        {
            if (openFileDialog.ShowDialog() != true)
                return;

            Task.Run(() =>
            {
                try
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ImportCustomFile = openFileDialog.FileName;

                        InputData = ExcelFileHandler.Import(openFileDialog.FileName);

                        LoadSourceValues(InputData.Columns.Cast<DataColumn>().Select(i => i.ColumnName));
                    });
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }
            });
        }

        private void LoadSourceValues(IEnumerable<string> cols)
        {
            SourceValues.Clear();

            SourceValues.Add("");

            foreach (var item in cols)
                SourceValues.Add(item);
        }

        public void MapValues()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Processing input file...", MessageType.Information));

                    var mappingSetup = importMapping.ToArray();

                    var transType = Enum.GetValues(typeof(TK.TransactionType)).Cast<TK.TransactionType>();

                    foreach (DataRow row in InputData.Rows)
                    {
                        var rawData = new RawData();

                        for (int a = 0; a < mappingSetup.Length; a++)
                        {
                            var mapping = mappingSetup[a];

                            var rowValue = row[mapping.Source].ToString();

                            rowValue = mapping[rowValue];

                            var targetField = mapping.Target.Replace(" ", "").Replace("/", "");

                            var propInfo = typeof(RawData).GetProperty(targetField);

                            if (propInfo.PropertyType == typeof(int))
                            {
                                var value = 0;

                                if (string.Compare(rowValue, "in", true) == 0)
                                    value = 1;
                                else if (string.Compare(rowValue, "out", true) == 0)
                                    value = 2;

                                propInfo.SetValue(rawData, Convert.ToInt16(value));
                            }
                            else if (propInfo.PropertyType == typeof(DateTime))
                            {
                                propInfo.SetValue(rawData, DateTime.Parse(rowValue));
                            }
                            else
                            {
                                propInfo.SetValue(rawData, rowValue);
                            }
                        }

                        OutputData.Add(rawData);
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Input file mapped. Please see output tab for results", MessageType.Success));

                    NotifyOfPropertyChange(() => OutputData);
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void ImportCustom()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving mapped data...", MessageType.Information));

                    for (int i = 0; i < outputData.Count; i++)
                    {
                        RawData rawData = outputData[i];

                        try
                        {
                            eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving {rawData.BiometricsId} - " +
                                                                                  $"{rawData.ScheduleDate.ToShortDateString()}...", MessageType.Information));
                            service.Save(mapper.Map<TK.RawData>(rawData));
                        }
                        catch (Exception ex)
                        {
                            eventAggregator.PublishOnUIThread(new NewMessageEvent($"Error on saving {rawData.BiometricsId} - " +
                                                                                  $"{rawData.ScheduleDate.ToShortDateString()}...", MessageType.Error));
                        }
                    }

                    Populate();

                    EndEditing();

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Mapped data has been saved", MessageType.Success));
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

            if (splitValue.Any(str => entity.TransactionDateTime.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.TransactionDateTime.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(RawData.ScheduleDate), System.ComponentModel.ListSortDirection.Descending));
            View.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(RawData.TransactionDateTime), System.ComponentModel.ListSortDirection.Ascending));
        }

        public void Dispose()
        {
            service.Dispose();
        }

        public string ImportCustomFile
        {
            get => string.IsNullOrEmpty(importFile) ? "Click here to select a file" : importFile;
            set
            {
                importFile = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<ExcelMappingModel> ImportMapping
        {
            get => importMapping ?? (importMapping = new ObservableCollection<ExcelMappingModel>());
            set
            {
                importMapping = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<string> SourceValues
        {
            get => sourceValues ?? (sourceValues = new ObservableCollection<string>() { "" });
            set
            {
                sourceValues = value;
                NotifyOfPropertyChange();
            }
        }

        public DataTable InputData
        {
            get => inputData;
            set
            {
                inputData = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<RawData> OutputData
        {
            get => outputData ?? (outputData = new ObservableCollection<RawData>());
            set
            {
                outputData = value;
                NotifyOfPropertyChange();
            }
        }
    }

    public interface IBiometricsIntegrationAPI
    {

    }
}

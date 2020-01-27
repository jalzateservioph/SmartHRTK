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
        readonly EmployeeService empservice;

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
            empservice = new EmployeeService(Session.Default.CurrentUser.Id);

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

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleting records...", 0));

                try
                {
                    int count = 0;

                    var forDelete = Items.Where(i => i.IsSelected).ToArray();

                    foreach (var item in forDelete)
                    {
                        service.DeleteHard(mapper.Map<TK.RawData>(item));

                        Items.Remove(item);

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
                        service.Import(openFileDialog.FileName, data =>
                        {
                            RaiseMessage($"Import {data.BiometricsId} - {data.ScheduleDate.ToShortDateString()} - {data.TransactionDateTime.ToShortTimeString()}");
                            Items.Add(mapper.Map<RawData>(data));
                        });

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

                    var rows = InputData.Rows.Cast<DataRow>().ToArray();

                    for (int ctr = 0; ctr < rows.Length; ctr++)
                    {
                        RawData rawData = null;

                        try
                        {
                            var row = rows[ctr];

                            if (row[0] == null || string.IsNullOrEmpty(row[0].ToString()))
                                continue;
                            if (row[1] == null || string.IsNullOrEmpty(row[1].ToString()))
                                continue;
                            if (row[2] == null || string.IsNullOrEmpty(row[2].ToString()))
                                continue;
                            if (row[3] == null || string.IsNullOrEmpty(row[3].ToString()))
                                continue;

                            var biometricsId = row[mappingSetup.First(i => i.Target.Replace(" ", "") == "BiometricsId").Source].ToString();
                            var transactionType = row[mappingSetup.First(i => i.Target.Replace(" ", "") == "TransactionType").Source].ToString().FindFrom("in", "out", "c\\in", "c\\out").Contains("in") ? 1 : 2;
                            var transactionDateTime = DateTimeHelpers.Parse(row[mappingSetup.First(i => i.Target.Replace(" ", "").Replace("/", "") == "TransactionDateTime").Source].ToString()).Value;
                            var scheduleDate = DateTimeHelpers.Parse(row[mappingSetup.First(i => i.Target.Replace(" ", "") == "ScheduleDate").Source].ToString()).Value;



                            rawData = new RawData()
                            {
                                BiometricsId = biometricsId,
                                TransactionType = transactionType,
                                TransactionDateTime = transactionDateTime,
                                ScheduleDate = scheduleDate
                            };

                            //for (int a = 0; a < mappingSetup.Length; a++)
                            //{
                            //    var mapping = mappingSetup[a];

                            //    var rowValue = row[mapping.Source].ToString();

                            //    rowValue = mapping[rowValue];

                            //    var targetField = mapping.Target.Replace(" ", "").Replace("/", "");

                            //    var propInfo = typeof(RawData).GetProperty(targetField);

                            //    if (propInfo.PropertyType == typeof(int))
                            //    {
                            //        var value = 0;

                            //        if (string.Compare(rowValue, "in", true) == 0)
                            //            value = 1;
                            //        else if (string.Compare(rowValue, "out", true) == 0)
                            //            value = 2;

                            //        propInfo.SetValue(rawData, Convert.ToInt16(value));
                            //    }
                            //    else if (propInfo.PropertyType == typeof(DateTime))
                            //    {
                            //        if (DateTime.TryParse(rowValue, out DateTime dt))
                            //        {
                            //            propInfo.SetValue(rawData, dt);
                            //        }
                            //        else
                            //        {
                            //            propInfo.SetValue(rawData, DateTime.FromOADate(double.Parse(rowValue)));
                            //        }
                            //    }
                            //    else
                            //    {
                            //        propInfo.SetValue(rawData, rowValue);
                            //    }
                            //}

                            OutputData.Add(rawData);
                        }
                        catch (Exception ex)
                        {
                            HandleError($"Error in row {ctr + 1} - {rawData.BiometricsId} - {rawData.ScheduleDate.ToShortDateString()} - {ex.Message}");
                        }
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

                    if (OutputData == null || OutputData.Count == 0)
                        throw new Exception("Please map data first.");

                    RaiseMessage($"Saving mapped data...", MessageType.Information);

                    int total = outputData.Count;

                    for (int i = 0; i < outputData.Count; i++)
                    {
                        RawData rawData = outputData[i];

                        try
                        {
                            RaiseMessage($"Saving {rawData.BiometricsId} - {rawData.ScheduleDate.ToShortDateString()} ({i + 1}/{total})...", MessageType.Information);

                            service.SaveNoAdjustment(mapper.Map<TK.RawData>(rawData));
                        }
                        catch (AppException ex)
                        {
                            ex.ErrorMessage = $"Error on saving {rawData.BiometricsId} - {rawData.ScheduleDate.ToShortDateString()}...";

                            throw;
                        };
                    }

                    Populate();

                    EndEditing();

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Mapped data has been saved", MessageType.Success));
                }
                catch (AppException ex)
                {
                    HandleError(ex, "RawDataViewModel");
                }
                catch (Exception ex)
                {
                    HandleError(ex, "RawDataViewModel");
                }

                EndProcessing();
            });
        }

        public override void EndEditing()
        {
            base.EndEditing();

            InputData?.Clear();
            OutputData?.Clear();
            ImportCustomFile = "";
        }

        public void DownloadTemplate()
        {
            if (saveFileDialog.ShowDialog() == true)
            {
                service.ExportTemplate(saveFileDialog.FileName);

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saved file to {saveFileDialog.FileName}", MessageType.Success));
            }
        }

        public void DownloadData()
        {
            if (saveFileDialog.ShowDialog() == true)
            {
                StartProcessing();

                List<TimesheetEntry> timesheetentries = new List<TimesheetEntry>();

                TimesheetEntry entry = null;

                foreach (var record in View.Cast<RawData>())
                {
                    var emp = empservice.List().FirstOrDefault(i => i.BiometricsId == record.BiometricsId)?.ToString();

                    if (string.IsNullOrEmpty(emp))
                    {
                        entry = null;
                        continue;
                    }

                    entry = timesheetentries.FirstOrDefault(i => i.BiometricsId == record.BiometricsId && i.ScheduleDate.Date == record.ScheduleDate.Date);

                    if (entry == null)
                    {
                        entry = new TimesheetEntry
                        {
                            Employee = empservice.List().FirstOrDefault(i => i.BiometricsId == record.BiometricsId).ToString(),
                            BiometricsId = record.BiometricsId,
                            ScheduleDate = record.ScheduleDate
                        };

                        timesheetentries.Add(entry);
                    }

                    if (record.ScheduleDate.Date == entry.ScheduleDate.Date)
                    {
                        if (record.TransactionType == 1) // in 
                        {
                            if (entry.TimeIn == DateTime.MinValue || record.TransactionDateTime < entry.TimeIn)
                                entry.TimeIn = record.TransactionDateTime;
                        }
                        if (record.TransactionType == 2) // out
                        {
                            if (entry.TimeOut == DateTime.MinValue || record.TransactionDateTime > entry.TimeOut)
                                entry.TimeOut = record.TransactionDateTime;
                        }
                    }
                }

                var data = DataTableHelpers.ToStringDataTable(timesheetentries);


                ExcelFileHandler.Export(saveFileDialog.FileName, data);

                RaiseMessage($"Saved file to {saveFileDialog.FileName}", MessageType.Success);

                EndProcessing();
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

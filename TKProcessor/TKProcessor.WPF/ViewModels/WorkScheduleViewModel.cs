using AutoMapper;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    public class WorkScheduleViewModel : EditableViewModelBase<WorkSchedule>, IDisposable
    {
        readonly IMapper mapper;
        readonly WorkScheduleService service;
        readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        private ObservableCollection<Shift> _shiftList;
        private ObservableCollection<Employee> _employeeList;
        private WorkSchedule _currentItem;
        private bool isNewRecordState;

        public WorkScheduleViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            PropertyChanged += WorkScheduleViewModel_PropertyChanged;

            service = new WorkScheduleService(Session.Default.CurrentUser.Id) { AutoSaveChanges = true };

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkSchedule, TK.WorkSchedule>();
                cfg.CreateMap<TK.WorkSchedule, WorkSchedule>();

                cfg.CreateMap<Employee, TK.Employee>();
                cfg.CreateMap<TK.Employee, Employee>();

                cfg.CreateMap<Shift, TK.Shift>();
                cfg.CreateMap<TK.Shift, Shift>();

                cfg.CreateMap<User, TK.User>();
                cfg.CreateMap<TK.User, User>();

                cfg.CreateMap<WorkSchedule, WorkSchedule>();
                cfg.CreateMap<WorkSchedule, WorkSchedule>();

                cfg.CreateMap<TK.WorkSite, WorkSite>();
                cfg.CreateMap<WorkSite,TK.WorkSite>();

                cfg.CreateMap<Employee, Employee>();
                cfg.CreateMap<Employee, Employee>();

                cfg.CreateMap<Shift, Shift>();
                cfg.CreateMap<Shift, Shift>();

                cfg.CreateMap<User, User>();
                cfg.CreateMap<User, User>();

            }).CreateMapper();

            openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx"
            };

            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Excel file (*.xlsx)|*.xlsx"
            };

            Populate();
        }

        private void WorkScheduleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsCheckedAll))
                View.Cast<WorkSchedule>().ToList().ForEach(i => i.IsSelected = IsCheckedAll);
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    CurrentItem = new WorkSchedule();

                    using (ShiftService service = new ShiftService(Session.Default.CurrentUser.Id))
                    {
                        ShiftList = new ObservableCollection<Shift>();

                        foreach (var item in service.List())
                        {
                            App.Current.Dispatcher.Invoke(() => { ShiftList.Add(mapper.Map<Shift>(item)); });
                        }
                    }

                    using (EmployeeService service = new EmployeeService(Session.Default.CurrentUser.Id))
                    {
                        EmployeeList = new ObservableCollection<Employee>();

                        foreach (var item in service.List())
                        {
                            var emp = mapper.Map<Employee>(item);

                            emp.IsSelected = false;
                            emp.IsDirty = false;
                            emp.IsValid = true;

                            App.Current.Dispatcher.Invoke(() => { EmployeeList.Add(emp); });
                        }
                    }

                    Items.Clear();

                    foreach (var item in service.List().Where(i => i.IsActive))
                    {
                        var ws = mapper.Map<WorkSchedule>(item);

                        Items.Add(ws);
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} raw data entries ", MessageType.Success));
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
                    bool? diagRes = null;

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        diagRes = openFileDialog.ShowDialog();
                    });

                    if (diagRes == true)
                    {
                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Importing {openFileDialog.FileName}...", MessageType.Success));

                        service.Import(openFileDialog.FileName, ws =>
                        {
                            eventAggregator.PublishOnUIThread(new NewMessageEvent(ws + "...", MessageType.Success));
                        });

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
                StartProcessing();

                try
                {
                    foreach (var item in service.HeaderDef)
                    {
                        var suffix = "";

                        if (item.Any(i => i == "{datetime}"))
                        {
                            suffix = "_plot";
                            item[1] = DateTime.Today.ToShortDateString();
                        }
                        else if (item.Any(i => i == "To" || i == "From"))
                            suffix = "_summary";

                        ExcelFileHandler.Export($"{Path.GetDirectoryName(saveFileDialog.FileName)}" +
                                                $"\\{Path.GetFileNameWithoutExtension(saveFileDialog.FileName)}{suffix}.xlsx", item);
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Successfully downloaded template files", MessageType.Success));
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }

                EndProcessing();
            }
        }

        public void New()
        {
            CurrentItem = new WorkSchedule()
            {
                ScheduleDate = DateTime.Today,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

            IsNewRecordState = true;

            StartEditing();
        }

        public void Edit()
        {
            if (ActiveItem == null)
                return;

            CurrentItem = mapper.Map<WorkSchedule>(ActiveItem);

            CurrentItem.Employee = EmployeeList.FirstOrDefault(i => i.Id == ActiveItem.Employee.Id);

            CurrentItem.Shift = ShiftList.FirstOrDefault(i => i.Id == ActiveItem.Shift.Id);

            IsNewRecordState = false;

            StartEditing();
        }

        public void Save()
        {
            if (IsAdvancedMode)
            {
                var wsObject = CurrentItem;

                Task.Run(() =>
                {
                    StartProcessing();

                    try
                    {
                        if (wsObject.Employee == null)
                            throw new Exception("Please select an employee");

                        Shift[] shifts = new Shift[] {
                            wsObject.Sunday,
                            wsObject.Monday,
                            wsObject.Tuesday,
                            wsObject.Wednesday,
                            wsObject.Thursday,
                            wsObject.Friday,
                            wsObject.Saturday
                        };

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving work schedules for {wsObject.Employee} ", MessageType.Information));

                        var start = wsObject.StartDate;
                        var end = wsObject.EndDate;

                        while (start <= end)
                        {
                            WorkSchedule forSave = null;

                            try
                            {
                                if (shifts[(int)start.DayOfWeek] == null)
                                {
                                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Skipping because {start.DayOfWeek.ToString()} " +
                                        $"has no shift setup", MessageType.Information));

                                    start = start.AddDays(1);

                                    continue;
                                }

                                forSave = new WorkSchedule()
                                {
                                    Employee = wsObject.Employee,
                                    ScheduleDate = start,
                                    Shift = shifts[(int)start.DayOfWeek]
                                };

                                eventAggregator.PublishOnUIThread(
                                   new NewMessageEvent(
                                       $"Saving {forSave.ScheduleDate.ToShortDateString()} - {forSave.Shift}",
                                       MessageType.Information
                                   )
                               );

                                service.Save(mapper.Map<TK.WorkSchedule>(forSave));

                                var existing = Items.FirstOrDefault(i => i.Employee.Id == forSave.Employee.Id &&
                                                                         i.ScheduleDate.Date == forSave.ScheduleDate.Date);

                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    if (existing != default(WorkSchedule))
                                        Items.Remove(existing);

                                    Items.Add(forSave);
                                });
                            }
                            catch (Exception ex)
                            {
                                eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                            }

                            start = start.AddDays(1);
                        }

                        eventAggregator.PublishOnUIThread(
                            new NewMessageEvent(
                                $"Saved work schedule for {wsObject.Employee} from " +
                                $"{wsObject.StartDate.ToShortDateString()} to {wsObject.EndDate.ToShortDateString()}",
                                MessageType.Success
                            )
                        );

                        EndEditing();
                    }
                    catch (Exception ex)
                    {
                        eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                    }

                    EndProcessing();
                });
            }
            else
            {
                try
                {
                    var forSave = new WorkSchedule()
                    {
                        Id = CurrentItem.Id,
                        ScheduleDate = CurrentItem.ScheduleDate,
                        Employee = CurrentItem.Employee,
                        Shift = CurrentItem.Shift
                    };

                    var existing = Items.FirstOrDefault(i => i.Employee.Id == forSave.Employee.Id &&
                                                             i.ScheduleDate.Date == forSave.ScheduleDate.Date);

                    service.Save(mapper.Map<TK.WorkSchedule>(forSave));

                    if (existing != default(WorkSchedule))
                        Items.Remove(existing);

                    Items.Add(forSave);

                    eventAggregator.PublishOnUIThread(
                        new NewMessageEvent(
                            $"Saved work schedule for {CurrentItem.Employee} for " +
                            $"{CurrentItem.ScheduleDate.ToShortDateString()}",
                            MessageType.Success
                        )
                    );

                    EndEditing();
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }
            }
        }

        public void Delete()
        {
            Task.Run(() =>
            {
                StartProcessing();

                foreach (var item in Items.Where(i => i.IsSelected))
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleting {item.Employee} - {item.ScheduleDate.ToShortDateString()}", MessageType.Error));
                    service.Delete(mapper.Map<TK.WorkSchedule>(item));
                }

                EndProcessing();
            });
        }

        public void Dispose()
        {
            service.Dispose();
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription("Employee.EmployeeCode", ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription("ScheduleDate", ListSortDirection.Descending));
        }

        public override bool Filter(object o)
        {
            if (string.IsNullOrEmpty(FilterString))
                return true;

            string[] filterGroups = FilterString.Split(';')
                                                .Where(i => !string.IsNullOrEmpty(i))
                                                .ToArray();

            WorkSchedule entity = (WorkSchedule)o;

            bool[] result = new bool[filterGroups.Length];

            string[] filterColumns = new string[]
            {
                entity.Employee.EmployeeCode.ToLower(),
                entity.Employee.FullName.ToLower(),
                entity.Shift.ShiftCode.ToLower(),
                entity.ScheduleDate.ToLongDateString(),
                entity.ScheduleDate.ToString("MM/dd/yyyy"),
                entity.ScheduleDate.ToString("MM-dd-yyyy"),
                entity.ScheduleDate.ToString("hh:mm")
            };

            for (int a = 0; a < filterGroups.Length; a++)
            {
                var filters = filterGroups[a].Split(',')
                                             .Where(i => !string.IsNullOrEmpty(i))
                                             .ToArray();

                result[a] = (filters.Length > 1);

                int counter = 0;

                foreach (var col in filterColumns)
                {
                    if (filters.Any(i => col.Contains(i.Trim().ToLower())))
                        counter++;
                }

                result[a] = (counter >= filters.Length);
            }

            return result.Any(i => i);
        }

        public ObservableCollection<Shift> ShiftList
        {
            get => _shiftList;
            set
            {
                _shiftList = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get => _employeeList;
            set
            {
                _employeeList = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsAdvancedMode { get; set; }

        public bool IsNewRecordState
        {
            get => isNewRecordState;
            set
            {
                isNewRecordState = value;
                NotifyOfPropertyChange();
            }
        }

        public WorkSchedule CurrentItem
        {
            get => _currentItem ?? (_currentItem = new WorkSchedule());
            set
            {
                _currentItem = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

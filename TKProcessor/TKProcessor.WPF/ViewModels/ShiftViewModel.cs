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
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKModels = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class ShiftViewModel : EditableViewModelBase<Shift>
    {
        readonly IMapper mapper;
        readonly OpenFileDialog openFileDialog;
        readonly SaveFileDialog saveFileDialog;
        readonly ShiftService service;
        private Shift _currentItem;

        public ShiftViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
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

            Populate();
        }

        private void ShiftViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ActiveItem))
                CurrentItem = mapper.Map<Shift>(ActiveItem);
            if (e.PropertyName == nameof(IsCheckedAll))
                View.Cast<Shift>().ToList().ForEach(i => i.IsSelected = IsCheckedAll);
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    IsCheckedAll = false;

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieving shifts from the database...", 0));

                    Items.Clear();

                    foreach (var item in service.List().Where(i => i.IsActive).Select(i => mapper.Map<Shift>(i)))
                    {
                        Items.Add(item);
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} shifts ", MessageType.Success));
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
            StartProcessing();

            try
            {
                if (string.IsNullOrEmpty(CurrentItem.ShiftCode))
                    throw new Exception($"Cannot save shift without shift code");

                if (CurrentItem.ScheduleIn == null)
                    throw new Exception($"Cannot save shift without schedule in");

                if (CurrentItem.ScheduleOut == null)
                    throw new Exception($"Cannot save shift without schedule out");

                if (CurrentItem.RequiredWorkHours == 0)
                    throw new Exception($"Cannot save shift without required work hours");

                if (CurrentItem.NightDiffStart == null)
                    throw new Exception($"Cannot save shift without night differential start");

                if (CurrentItem.NightDiffEnd == null)
                    throw new Exception($"Cannot save shift without night differential end");

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

            CurrentItem = new Shift();

            using (var globalSettingsService = new GlobalSettingsService())
            {
                var a = globalSettingsService.List().Select(i => new { i.DefaultNDStart, i.DefaultNDEnd }).FirstOrDefault();

                if (a != null)
                {
                    CurrentItem.NightDiffStart = a.DefaultNDStart;
                    CurrentItem.NightDiffEnd = a.DefaultNDEnd;
                    CurrentItem.IsDirty = false;
                }
            }

            EndProcessing();

            StartEditing();
        }

        public void Edit()
        {
            StartEditing();
        }

        public void Delete()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleting shifts...", MessageType.Information));

                    foreach (var item in Items.Where(i => i.IsSelected).ToList())
                    {
                        service.Delete(mapper.Map<TKModels.Shift>(item));

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleting {item.ShiftCode}...", MessageType.Information));

                        Items.Remove(item);
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Deleted selected shifts.", MessageType.Information));

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

            string[] filterGroups = FilterString.Split(';')
                                                .Where(i => !string.IsNullOrEmpty(i))
                                                .ToArray();

            Shift entity = (Shift)o;

            bool[] result = new bool[filterGroups.Length];

            string[] filterColumns = new string[]
            {
                entity.ShiftCode.ToLower(),
                entity.Description.ToLower(),
                entity.ScheduleIn?.ToString("hh:mm"),
                entity.IsLateIn == true ? "late" : "nolate",
                entity.IsEarlyOut == true ? "early out" : "",
                entity.IsEarlyOut == true ? "undertime" : "",
                entity.IsRestDay == true ? "restday" : "",
                entity.IsPreShiftOt == true ? "preshiftot" : "",
                entity.IsPostShiftOt == true ? "postshiftot" : "",
                entity.IsHolidayRestDayOt == true ? "holidayrestdayoy" : ""
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

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription("ShiftCode", ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription("ScheduleIn", ListSortDirection.Ascending));
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

using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKModels = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class EmployeeViewModel : EditableViewModelBase<Employee>
    {
        readonly IMapper mapper;
        readonly EmployeeService employeeService;
        private Employee currentItem;
        private BindingList<WorkSite> workSiteList;

        public EmployeeViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, IMapper mapper) : base(eventAggregator, windowManager)
        {
            employeeService = new EmployeeService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            this.mapper = mapper;

            Init();

            Populate();
        }

        public void Init()
        {
            StartProcessing();

            RaiseMessage("Initializing...");

            foreach (var item in new WorkSiteService().Get())
            {
                WorkSiteList.Add(mapper.Map<WorkSite>(item));
            }

            InitializeFilter();

            EndProcessing();
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    RaiseMessage("Retrieving employees from the database...");

                    Items.Clear();

                    foreach (var item in employeeService.List())
                    {
                        Items.Add(mapper.Map<Employee>(item));

                        RaiseMessage($"Syncing {item.ToString()}...");
                    }

                    RaiseMessage($"Retrieved {Items.Count} employees.");
                }
                catch (Exception ex)
                {
                    HandleError(ex, GetType().Name);
                }

                EndProcessing();
            });
        }

        public override void InvokeFilter(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                View.Refresh();
            }
        }

        public void Sync()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    RaiseMessage("Employee sync started...");

                    employeeService.Sync(emp =>
                    {
                        var employee = mapper.Map<Employee>(emp);

                        var existing = Items.FirstOrDefault(i => i.Id == employee.Id ||
                                                                 i.EmployeeCode == employee.EmployeeCode ||
                                                                 i.FullName == employee.FullName);

                        if (existing != default(Employee))
                            Items.Remove(existing);

                        Items.Add(employee);

                        RaiseMessage($"Syncing {employee.ToString()}...");
                    });

                    employeeService.SaveChanges();


                    RaiseMessage($"Syncing leaves...");

                    new LeaveService(Session.Default.CurrentUser.Id).Sync();

                    RaiseMessage("Employee Sync Complete!");
                }
                catch (Exception ex)
                {
                    HandleError(ex, GetType().Name);
                }

                EndProcessing();
            });
        }

        public void Save()
        {
            try
            {
                StartProcessing();

                RaiseMessage($"Saving {CurrentItem}...");

                foreach (var site in currentItem.EmployeeWorkSites)
                {
                    if (site.Id == Guid.Empty)
                        site.Id = Guid.NewGuid();

                    site.WorkSiteId = site.WorkSite.Id;

                    site.Employee = currentItem;
                    site.EmployeeId = currentItem.Id;
                }

                employeeService.Save(mapper.Map<TKModels.Employee>(CurrentItem));

                if (employeeService.SaveChanges() > 0)
                    RaiseMessage($"Employee record saved");
                else
                    RaiseMessage($"Record was not saved", MessageType.Warning);

                Items.First(i => i.Id == CurrentItem.Id).BiometricsId = CurrentItem.BiometricsId;

                EndProcessing();
            }
            catch (Exception ex)
            {
                HandleError(ex, GetType().Name);
            }
        }

        public void OpenRecord(Employee employee)
        {
            foreach (var item in employee.EmployeeWorkSites)
            {
                item.WorkSite = workSiteList.First(i => i.Id == item.WorkSiteId);
            }

            CurrentItem = employee;

            StartEditing();
        }

        public override bool Filter(object o)
        {
            if (string.IsNullOrEmpty(FilterString))
                return true;

            string[] filterGroups = FilterString.Split(';')
                                                .Where(i => !string.IsNullOrEmpty(i))
                                                .ToArray();

            Employee entity = (Employee)o;

            bool[] result = new bool[filterGroups.Length];

            string[] filterColumns = new string[]
            {
                entity.EmployeeCode.ToLower(),
                entity.FullName.ToLower()
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
            View.SortDescriptions.Add(new SortDescription("EmployeeCode", ListSortDirection.Ascending));
        }

        public Employee CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                NotifyOfPropertyChange();
            }
        }

        public BindingList<WorkSite> WorkSiteList
        {
            get => workSiteList ?? (workSiteList = new BindingList<WorkSite>());
            set
            {
                workSiteList = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

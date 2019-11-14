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
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
using TKProcessor.WPF.Events;
using TKProcessor.WPF.Models;
using TKModels = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class EmployeeViewModel : ViewModelBase<Employee>
    {
        readonly IMapper mapper;
        readonly EmployeeService employeeService;

        public EmployeeViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            employeeService = new EmployeeService(Session.Default.CurrentUser?.Id ?? Guid.Empty);

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, TKModels.Employee>();
                cfg.CreateMap<TKModels.Employee, Employee>()
                    .AfterMap((empDto, emp) => emp.IsDirty = false);

                cfg.CreateMap<User, TKModels.User>();
                cfg.CreateMap<TKModels.User, User>();
            }).CreateMapper();

            Populate();
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent("Retrieving employees from the database..."));

                    Items.Clear();

                    foreach (var item in employeeService.List())
                    {
                        Items.Add(mapper.Map<Employee>(item));

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Syncing {item.ToString()}..."));
                    }

                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Retrieved {Items.Count} employees."));
                }
                catch (Exception ex)
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
                }

                EndProcessing();
            });
        }

        public void Sync()
        {
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent("Employee sync started..."));

                    employeeService.Sync(Session.Default.CurrentUser.Id, emp =>
                    {
                        var employee = mapper.Map<Employee>(emp);

                        var existing = Items.FirstOrDefault(i => i.Id == employee.Id ||
                                                                 i.EmployeeCode == employee.EmployeeCode ||
                                                                 i.FullName == employee.FullName);

                        if (existing != default(Employee))
                            Items.Remove(existing);

                        Items.Add(employee);

                        eventAggregator.PublishOnUIThread(new NewMessageEvent($"Syncing {employee.ToString()}..."));
                    });

                    eventAggregator.PublishOnUIThread(new NewMessageEvent("Employee Sync Complete!"));
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
                StartProcessing();

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving edited employees..."));

                foreach (var employee in Items.Where(i => i.IsDirty))
                {
                    eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving {employee}..."));

                    employeeService.Save(mapper.Map<TKModels.Employee>(employee));

                    employee.IsDirty = false;
                }

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Employee records saved"));

                EndProcessing();
            }
            catch (Exception ex)
            {
                eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
            }
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
    }
}

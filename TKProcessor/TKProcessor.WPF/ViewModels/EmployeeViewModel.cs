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
        readonly EmployeeService service;

        public EmployeeViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new EmployeeService(Session.Default.CurrentUser?.Id ?? Guid.Empty);
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

                    foreach (var item in service.List())
                        Items.Add(mapper.Map<Employee>(item));

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

                    service.Sync(Session.Default.CurrentUser.Id);

                    eventAggregator.PublishOnUIThread(new NewMessageEvent("Employee Sync Complete!"));

                    Populate();
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

                eventAggregator.PublishOnUIThread(new NewMessageEvent($"Saving employees..."));

                foreach (var employee in Items.Where(i => i.IsDirty))
                {
                    service.Save(mapper.Map<TKModels.Employee>(employee));
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
            if (FilterString == "")
                return true;

            var filters = FilterString.Split(',');
            var entity = (Employee)o;

            if (filters.Any(i => entity.EmployeeCode.ToLower().Contains(i.Trim().ToLower())))
                return true;

            if (filters.Any(i => entity.FullName.ToLower().Contains(i.Trim().ToLower())))
                return true;

            return false;
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new SortDescription("Employee.EmployeeCode", ListSortDirection.Ascending));
            View.SortDescriptions.Add(new SortDescription("Employee.BiometricsId", ListSortDirection.Ascending));
        }
    }
}

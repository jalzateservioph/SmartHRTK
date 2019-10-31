using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using TKProcessor.Services;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class AuditLogsViewModel : ViewModelBase<AuditLog>
    {
        readonly TKService<TKProcessor.Models.TK.AuditLog> service;
        readonly IMapper mapper;

        public AuditLogsViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new TKService<TKProcessor.Models.TK.AuditLog>();
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TKProcessor.Models.TK.AuditLog, AuditLog>();
                
                cfg.CreateMap<TKProcessor.Models.TK.User, User>();
            }).CreateMapper();

            Populate();
        }

        public void Populate()
        {
            Task.Run(() =>
            {
                Items.Clear();

                foreach (var item in service.List())
                {
                    Items.Add(mapper.Map<AuditLog>(item));
                }
            });
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new System.ComponentModel.SortDescription("ModifiedOn", System.ComponentModel.ListSortDirection.Descending));
        }

        public override bool Filter(object o)
        {
            var entity = o as AuditLog;
            var splitValue = FilterString.Split(',');

            if (splitValue.Any(str => entity.Target == null ? false : entity.Target.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.ModifiedBy == null ? false : entity.ModifiedBy.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.ModifiedOn == null ? false : entity.ModifiedOn.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.ModifiedOn == null ? false : entity.ModifiedOn.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.ModifiedOn == null ? false : entity.ModifiedOn.ToLongTimeString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.ModifiedOn == null ? false : entity.ModifiedOn.ToShortTimeString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }
    }
}

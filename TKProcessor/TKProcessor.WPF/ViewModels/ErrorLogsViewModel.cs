using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Services;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class ErrorLogsViewModel : ViewModelBase<ErrorLog>, IDisposable
    {
        readonly TKService<TKProcessor.Models.TK.ErrorLog> service;
        readonly IMapper mapper;

        public ErrorLogsViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new TKService<TKProcessor.Models.TK.ErrorLog>();
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TKProcessor.Models.TK.ErrorLog, ErrorLog>();
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
                    Items.Add(mapper.Map<ErrorLog>(item));
                }
            });
        }

        public override void Sort()
        {
            View.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateRaised", System.ComponentModel.ListSortDirection.Descending));
        }

        public override bool Filter(object o)
        {
            var entity = o as ErrorLog;
            var splitValue = FilterString.Split(',');

            if (splitValue.Any(str => entity.Source == null ? false : entity.Source.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.Message == null ? false : entity.Message.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.StackTrace == null ? false : entity.Message.ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.DateRaised == null ? false : entity.DateRaised.ToShortDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.DateRaised == null ? false : entity.DateRaised.ToLongDateString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.DateRaised == null ? false : entity.DateRaised.ToLongTimeString().ToLower().Contains(str.ToLower())))
                return true;

            if (splitValue.Any(str => entity.DateRaised == null ? false : entity.DateRaised.ToShortTimeString().ToLower().Contains(str.ToLower())))
                return true;

            return false;
        }

        public void Dispose()
        {
            service.Dispose();
        }
    }
}

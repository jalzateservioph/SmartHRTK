using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Models;
using TK = TKProcessor.Models.TK;

namespace TKProcessor.WPF.ViewModels
{
    public class WorkSiteViewModel : EditableViewModelBase<WorkSite>
    {
        readonly WorkSiteService service;

        readonly IMapper mapper;

        public WorkSiteViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            service = new WorkSiteService();

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TK.WorkSite, WorkSite>();
                cfg.CreateMap<WorkSite, TK.WorkSite>();

                cfg.CreateMap<TK.User, User>();
                cfg.CreateMap<User, TK.User>();
            }).CreateMapper();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        public void Populate()
        {
            foreach (var item in service.Get())
            {
                Items.Add(mapper.Map<WorkSite>(item));
            }
        }

        public void Save()
        {
            var existing = Items.FirstOrDefault(workSite => workSite.Id == ActiveItem.Id);

            if (existing == null)
                service.Add(mapper.Map<TK.WorkSite>(existing));
            else
                service.Update(existing.Id, mapper.Map<TK.WorkSite>(existing));
        }
    }
}

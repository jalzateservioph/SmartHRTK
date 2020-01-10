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
        private WorkSite currentItem;

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
            Task.Run(() =>
            {
                StartProcessing();

                try
                {
                    foreach (var item in service.Get())
                    {
                        Items.Add(mapper.Map<WorkSite>(item));
                    }
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }

                EndProcessing();
            });
        }

        public void ShowRecord(WorkSite entity = null)
        {
            CurrentItem = entity ?? new WorkSite();

            StartEditing();
        }

        public void Save()
        {
            try
            {
                var existing = Items.FirstOrDefault(workSite => workSite.Id == currentItem.Id);

                if (existing == null)
                    service.Add(mapper.Map<TK.WorkSite>(currentItem));
                else
                    service.Update(existing.Id, mapper.Map<TK.WorkSite>(existing));

                if (service.Save() > 0)
                    RaiseMessage("Record saved successfuly", Events.MessageType.Success);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        public void Delete()
        {

        }

       public void Import()
        {

        }

        public void Export()
        {

        }

        public void DownloadTemplate()
        {

        }

        public WorkSite CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                NotifyOfPropertyChange();
            }
        }
    }
}

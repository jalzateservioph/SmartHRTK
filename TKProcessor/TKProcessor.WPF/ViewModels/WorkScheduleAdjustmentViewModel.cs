using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class WorkScheduleAdjustmentViewModel : ViewModelBase<WorkScheduleAdjustment>
    {
        public WorkScheduleAdjustmentViewModel()
        {
            Initialize();
        }

        public WorkScheduleAdjustmentViewModel(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            Initialize();
        }

        public void Initialize()
        {

        }
    }
}

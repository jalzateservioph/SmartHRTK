using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class WorkScheduleAdjustment : BaseModel
    {
        public string Reason { get; set; }
        public WorkSchedule Target { get; set; }
        public Shift NewShift { get; set; }
    }
}

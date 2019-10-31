using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class WorkSchedule : Entity
    {
        public Employee Employee { get; set; }
        public DateTime ScheduleDate { get; set; }
        public Shift Shift { get; set; }
    }
}

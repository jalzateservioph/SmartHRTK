using Caliburn.Micro;
using System;

namespace TKProcessor.WPF.Models
{
    public class RawData : BaseModel
    {
        public string BiometricsId { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public DateTime ScheduleDate { get; set; }
    }

    public class TimesheetEntry
    {
        public string Employee { get; set; }
        public string BiometricsId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}
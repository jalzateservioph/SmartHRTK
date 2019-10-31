using Caliburn.Micro;
using System;

namespace TKProcessor.WPF.Models
{
    public class RawData : BaseModel
    {
        public string BiometricsId { get; set; }
        public int? TransactionType { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }
}
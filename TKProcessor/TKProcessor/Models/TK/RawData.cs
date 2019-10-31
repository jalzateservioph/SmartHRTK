using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class RawData : Entity
    {
        public string BiometricsId { get; set; }
        public int? TransactionType { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }

    public enum TransactionType
    {
        TimeIn = 1,
        TimeOut = 2,
        AMBreakIn = 3,
        AMBreakOut = 4,
        LunchIn = 5,
        LunchOut = 6,
        PMBreakIn = 7,
        PMBreakOut = 8,
        DinnerIn = 9,
        DinnerOut = 10
    }

}

using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class Shift : Entity
    {
        public override string ToString()
        {
            return ShiftCode;
        }

        public string ShiftCode { get; set; }
        public string Description { get; set; }
        public int? ShiftType { get; set; }
        public int? FocusDate { get; set; }
        public DateTime? ScheduleIn { get; set; }
        public DateTime? ScheduleOut { get; set; }
        public decimal? RequiredWorkHours { get; set; }
        public bool? IsRestDay { get; set; }
        public DateTime? NightDiffStart { get; set; }
        public DateTime? NightDiffEnd { get; set; }
        public DateTime? AmbreakIn { get; set; }
        public DateTime? AmbreakOut { get; set; }
        public DateTime? PmbreakIn { get; set; }
        public DateTime? PmbreakOut { get; set; }
        public DateTime? LunchIn { get; set; }
        public DateTime? LunchOut { get; set; }
        public DateTime? DinnerIn { get; set; }
        public DateTime? DinnerOut { get; set; }
        public bool? IsLateIn { get; set; }
        public int? GracePeriodLateIn { get; set; }
        public int? AfterEvery { get; set; }
        public int? DeductionLateIn { get; set; }
        public bool? IsPlusLateInMinutes { get; set; }
        public int? MaximumMinutesConsideredAsHalfDay { get; set; }
        public bool? IsEarlyOut { get; set; }
        public int? GracePeriodEarlyOut { get; set; }
        public decimal? AfterEveryEarlyOut { get; set; }
        public int? DeductionOfEarlyOut { get; set; }
        public bool? IsPlusEarlyOutMinutes { get; set; }
        public int? MaximumMinutesConsideredAsHalfAayEarlyOut { get; set; }
        public bool? IsPreShiftOt { get; set; }
        public int? MinimumPreShiftOt { get; set; }
        public int? MaximumPreShiftOt { get; set; }
        public int? RoundPreShiftOt { get; set; }
        public bool? IsPostShiftOt { get; set; }
        public int? MinimumPostShiftOt { get; set; }
        public int? MaximumPostShiftOt { get; set; }
        public int? RoundPostShiftOt { get; set; }
        public bool? IsHolidayRestDayOt { get; set; }
        public int? MinimumHolidayRestDayOt { get; set; }
        public int? MaximumHolidayRestDayOt { get; set; }
        public int? RoundHolidayRestDayOt { get; set; }
        public bool? IsOverbreak { get; set; }
        public int? FlextimeType { get; set; }
        public DateTime? EarliestTimeIn { get; set; }
        public DateTime? LatestTimeIn { get; set; }
        public DateTime? EarliestTimeOut { get; set; }
        public DateTime? LatestTimeOut { get; set; }
        public int? Increment { get; set; }

    }
    public enum ShiftType
    {
        Standard, Flex
    }

    public enum FlextimeType
    {
        Full, SemiOnTheDot, SemiFixedIncrements
    }

    public enum Increment
    {
        HalfHour, WholeHour
    }

    public enum FocusDate
    {
        ScheduleIn, ScheduleOut
    }
}

using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public class Calendar : Entity
    {
        public string Name { get; set; }
        ICollection<CalendarDay> CalendarDays { get; set; }
    }

    public class CalendarDay : Entity
    {
        public DateTime Date { get; set; }
        public bool IsLegalHoliday { get; set; }
        public bool IsSpecialHoliday { get; set; }
    }

    public partial class Holiday : Entity
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
    }

    public enum HolidayType
    {
        Legal = 0, Special = 1
    }
}

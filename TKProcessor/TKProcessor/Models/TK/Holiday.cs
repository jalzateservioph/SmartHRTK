using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
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

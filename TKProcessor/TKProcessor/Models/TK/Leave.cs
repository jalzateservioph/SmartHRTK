using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class Leave : Entity
    {
        public string EmployeeCode { get; set; }
        public DateTime LeaveDate { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveHours { get; set; }
    }
}

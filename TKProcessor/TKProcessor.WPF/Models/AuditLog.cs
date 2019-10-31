using System;
using System.Collections.Generic;

namespace TKProcessor.WPF.Models
{
    public partial class AuditLog
    {
        public string Target { get; set; }
        public string Action { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}

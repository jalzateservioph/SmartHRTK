using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class AuditLog : IModel
    {
        public AuditLog()
        {
            Id = Guid.NewGuid();

            ModifiedOn = DateTime.Now;
        }

        public Guid Id { get; set; }


        public string Target { get; set; }
        public string Action { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}

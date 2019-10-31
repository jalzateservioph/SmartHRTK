using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class AuditLog : IEntity
    {
        public AuditLog()
        {
            Id = Guid.NewGuid();

            ModifiedOn = DateTime.Now;
        }

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }


        public string Target { get; set; }
        public string Action { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}

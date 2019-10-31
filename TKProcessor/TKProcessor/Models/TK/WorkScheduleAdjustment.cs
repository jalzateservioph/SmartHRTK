using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Models.TK
{
    class WorkScheduleAdjustment : IEntity
    {
        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }


        public string Reason { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}

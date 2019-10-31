using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class PayrollTrxLines
    {
        public Guid SeqId { get; set; }
        public Guid PayrollTrxSeqId { get; set; }
        public short DisplayOrder { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime LineDate { get; set; }
        public DateTime? VoidDate { get; set; }
        public string ExtRefTrx { get; set; }
        public string ExtRefLine { get; set; }
        public string PayPackageCode { get; set; }
        public string PayFreqCalendarCode { get; set; }
        public string PayrollCodeCode { get; set; }
        public string AttributeCode { get; set; }
        public string InputValue { get; set; }
        public byte PostingAction { get; set; }
        public Guid? PayDayPayrollTrxSeqId { get; set; }
        public byte InstanceCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }

        public virtual PayrollTrx PayrollTrxSeq { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class PayrollTrx
    {
        public PayrollTrx()
        {
            PayrollTrxLines = new HashSet<PayrollTrxLines>();
        }

        public Guid SeqId { get; set; }
        public string CountryId { get; set; }
        public long TrxNo { get; set; }
        public byte Type { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DateTime RefDate { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? VoidDate { get; set; }
        public DateTime? DefaultLineDate { get; set; }
        public string DefaultExtRefTrx { get; set; }
        public string DefaultExtRefLine { get; set; }
        public string DefaultPayPackageCode { get; set; }
        public string DefaultPayFreqCalendarCode { get; set; }
        public string DefaultPayrollCodeCode { get; set; }
        public string DefaultAttributeCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }
        public byte[] Workbook { get; set; }

        public virtual ICollection<PayrollTrxLines> PayrollTrxLines { get; set; }
    }
}

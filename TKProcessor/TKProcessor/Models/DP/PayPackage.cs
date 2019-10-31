using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class PayPackage
    {
        public PayPackage()
        {
            PayPackagePayFreqCalendars = new HashSet<PayPackagePayFreqCalendars>();
        }

        public Guid SeqId { get; set; }
        public string CountryId { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool? Inactive { get; set; }
        public Guid? OriginatingCurrencySeqId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }

        public virtual ICollection<PayPackagePayFreqCalendars> PayPackagePayFreqCalendars { get; set; }
    }
}

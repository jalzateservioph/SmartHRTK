using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class PayPackagePayFreqCalendars
    {
        public Guid SeqId { get; set; }
        public Guid PayPackageSeqId { get; set; }
        public Guid PayFreqCalendarSeqId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }

        public virtual PayFreqCalendar PayFreqCalendarSeq { get; set; }
        public virtual PayPackage PayPackageSeq { get; set; }
    }
}

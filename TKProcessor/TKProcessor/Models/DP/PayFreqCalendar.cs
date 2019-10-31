using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class PayFreqCalendar
    {
        public PayFreqCalendar()
        {
            PayPackagePayFreqCalendars = new HashSet<PayPackagePayFreqCalendars>();
        }

        public Guid SeqId { get; set; }
        public string CountryId { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public byte PayFreqType { get; set; }
        public decimal? MinimumNetPay { get; set; }
        public bool? PerformRounding { get; set; }
        public byte? RoundingMethod { get; set; }
        public byte? RoundingDenomination { get; set; }
        public decimal? RoundingValue { get; set; }
        public Guid? CoinageSeqId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }

        public virtual ICollection<PayPackagePayFreqCalendars> PayPackagePayFreqCalendars { get; set; }
    }
}

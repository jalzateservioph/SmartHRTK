using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public partial class PayrollCode
    {
        public Guid SeqId { get; set; }
        public string CountryId { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool? PeriodicTotals { get; set; }
        public bool? Protected { get; set; }
        public bool? Inactive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }
    }
}

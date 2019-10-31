using System;
using System.Collections.Generic;

namespace TKProcessor.Models.DP
{
    public partial class GscuserSecurity
    {
        public Guid SeqId { get; set; }
        public Guid? GscrecordFilterSeqId { get; set; }
        public string CountryId { get; set; }
        public string UserCode { get; set; }
        public string UserPassword { get; set; }
        public string Name { get; set; }
        public string ErpuserId { get; set; }
        public bool? ProtectionSetting { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public long RowVersion { get; set; }
        public bool? Inactive { get; set; }
    }
}

using System;

namespace TKProcessor.WPF.Models
{
    public class EmployeeWorkSite
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid WorkSiteId { get; set; }

        public WorkSite WorkSite { get; set; }
    }
}

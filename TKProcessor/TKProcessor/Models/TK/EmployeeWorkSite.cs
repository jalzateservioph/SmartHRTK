using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Models.TK
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

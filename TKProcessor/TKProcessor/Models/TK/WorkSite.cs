using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public class WorkSite : IEntity
    {
        public WorkSite()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public string SiteId { get; set; }
        public string Name { get; set; }

        public string IntegrationAuthUsername { get; set; }
        public string IntegrationAuthPassword { get; set; }


        //public ICollection<Employee> AssignedEmployees { get; set; }

        public IList<EmployeeWorkSite> WorkSiteEmployees { get; set; }
    }
}

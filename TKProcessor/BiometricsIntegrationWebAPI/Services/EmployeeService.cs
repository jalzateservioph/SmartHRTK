using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;
namespace BiometricsIntegrationWebAPI.Services
{
    public class EmployeeService
    {
        private readonly TKContext context;

        public EmployeeService(TKContext context)
        {
            this.context = context;
        }

        public IEnumerable<Employee> GetEmployees(Guid siteId)
        {
            return context.Employee.Where(e => e.EmployeeWorkSites.Any( s => s.WorkSiteId == siteId));
            //return context.Employee.ToList();
        }
    }
}

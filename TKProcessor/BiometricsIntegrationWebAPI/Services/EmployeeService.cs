using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;
using TKServices = TKProcessor.Services.Maintenance;
namespace BiometricsIntegrationWebAPI.Services
{
    public class EmployeeService
    {
        private readonly TKServices.EmployeeService employeeService;

        public EmployeeService(TKContext context, TKAuthService authService)
        {
            employeeService = new TKServices.EmployeeService(authService.GetUser().Id, context);
        }

        public IEnumerable<Employee> GetEmployees(Guid siteId)
        {
            return employeeService.List().Where(e => e.EmployeeWorkSites.Any(s => s.WorkSiteId == siteId));
        }
    }
}

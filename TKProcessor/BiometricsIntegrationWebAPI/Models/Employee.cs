using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiometricsIntegrationWebAPI.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; }
        public string BiometricsId { get; set; }
        public string EmployeeName { get; set; }
    }
}

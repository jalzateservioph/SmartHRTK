using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationClient.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string BiometricsId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}

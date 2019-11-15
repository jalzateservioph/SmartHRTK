using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Models;
namespace IntegrationClient.DAL.Interfaces
{
    interface IEmployeeService
    {
        IEnumerable<Employee> GetEmployees();
    }
}

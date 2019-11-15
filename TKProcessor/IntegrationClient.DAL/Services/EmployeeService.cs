using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using IntegrationClient.DAL.Models;
namespace IntegrationClient.DAL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;
        public EmployeeService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
        } 


        public IEnumerable<Employee> GetEmployees()
        {
            return null; //To do
        }
    }
}

using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationClient.DAL.Services
{
    public class ZKTEcoDeviceService : IDeviceService
    {
        private IConfiguration _configuration;
        private ILoggingService _loggingService;

        public ZKTEcoDeviceService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
        }

        public IEnumerable<RawData> GetRawData(DateTime? from, DateTime? to)
        {
            return null;        
        }

        public void EnrollUsers(IEnumerable<Employee> employees) 
        {
            
        }
    }
}

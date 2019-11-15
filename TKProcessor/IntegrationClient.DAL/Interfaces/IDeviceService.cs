using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Models;
using Microsoft.Extensions.Configuration;

namespace IntegrationClient.DAL.Interfaces
{
    public interface IDeviceService
    {
        IEnumerable<RawData> GetRawData(DateTime? from, DateTime? to);

        void EnrollUsers(IEnumerable<Employee> employees);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Models;
namespace IntegrationClient.DAL.Interfaces
{
    public interface ITimekeepingService
    {
        IEnumerable<Employee> GetEmployees();
        void PushRawData(IEnumerable<RawData> rawData);
    }
}
